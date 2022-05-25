// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AzureIoTHub.Portal.Server.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AzureIoTHub.Portal.Models.v10;
    using AzureIoTHub.Portal.Shared.Constants;
    using Exceptions;
    using Microsoft.Azure.Devices;
    using Microsoft.Azure.Devices.Shared;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class DeviceService : IDeviceService
    {
        private readonly RegistryManager registryManager;
        private readonly ServiceClient serviceClient;
        private readonly ILogger<DeviceService> log;

        public DeviceService(
            ILogger<DeviceService> log,
            RegistryManager registryManager,
            ServiceClient serviceClient)
        {
            this.log = log;
            this.serviceClient = serviceClient;
            this.registryManager = registryManager;
        }

        /// <summary>
        /// this function return a list of all edge device wthiout tags.
        /// </summary>
        /// <param name="continuationToken"></param>
        /// <param name="searchText"></param>
        /// <param name="searchStatus"></param>
        /// <param name="searchType"></param>
        /// <param name="pageSize"></param>
        /// <returns>IEnumerable twin.</returns>
        public async Task<PaginationResult<Twin>> GetAllEdgeDevice(
            string continuationToken = null,
            string searchText = null,
            bool? searchStatus = null,
            string searchType = null,
            int pageSize = 10)
        {
            var filter = "WHERE devices.capabilities.iotEdge = true";

            if (searchStatus != null)
            {
                filter += $" AND status = '{(searchStatus.Value ? "enabled" : "disabled")}'";
            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
#pragma warning disable CA1308 // Normalize strings to uppercase
                filter += $" AND (STARTSWITH(deviceId, '{searchText.ToLowerInvariant()}') OR (is_defined(tags.deviceName) AND STARTSWITH(tags.deviceName, '{searchText}')))";
#pragma warning restore CA1308 // Normalize strings to uppercase
            }

            if (!string.IsNullOrWhiteSpace(searchType))
            {
                filter += $" AND devices.tags.type = '{searchType}'";
            }

            var emptyResult = new PaginationResult<Twin>
            {
                Items = Enumerable.Empty<Twin>(),
                TotalItems = 0
            };

            IEnumerable<string> count;

            try
            {
                count = await this.registryManager
                    .CreateQuery($"SELECT COUNT() as totalNumber FROM devices {filter}")
                    .GetNextAsJsonAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Unable to get the count of edge devices", e);
            }

            if (!JObject.Parse(count.Single()).TryGetValue("totalNumber", out var result))
            {
                return emptyResult;
            }

            if (result.Value<int>() == 0)
            {
                return emptyResult;
            }
            try
            {
                var response = await this.registryManager
                .CreateQuery($"SELECT * FROM devices { filter }", pageSize)
                .GetNextAsTwinAsync(new QueryOptions
                {
                    ContinuationToken = continuationToken
                });


                return new PaginationResult<Twin>

                {
                    Items = response,
                    TotalItems = result.Value<int>(),
                    NextPage = response.ContinuationToken
                };
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Unable to get edge devices", e);
            }
        }

        /// <summary>
        /// this function return a list of all device exept edge device.
        /// </summary>
        /// <param name="continuationToken"></param>
        /// <param name="filterDeviceType"></param>
        /// <param name="excludeDeviceType"></param>
        /// <param name="searchText"></param>
        /// <param name="searchStatus"></param>
        /// <param name="searchState"></param>
        /// <param name="searchTags"></param>
        /// <param name="pageSize"></param>
        /// <returns>IEnumerable twin.</returns>
        public async Task<PaginationResult<Twin>> GetAllDevice(
            string continuationToken = null,
            string filterDeviceType = null,
            string excludeDeviceType = null,
            string searchText = null,
            bool? searchStatus = null,
            bool? searchState = null,
            Dictionary<string, string> searchTags = null,
            int pageSize = 10)
        {
            var filter = "WHERE devices.capabilities.iotEdge = false";

            if (!string.IsNullOrWhiteSpace(filterDeviceType))
            {
                filter += $" AND devices.tags.deviceType = '{filterDeviceType}'";
            }

            if (!string.IsNullOrWhiteSpace(excludeDeviceType))
            {
                filter += $" AND (NOT is_defined(tags.deviceType) OR devices.tags.deviceType != '{excludeDeviceType}')";
            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
#pragma warning disable CA1308 // Normalize strings to uppercase
                filter += $" AND (STARTSWITH(deviceId, '{searchText.ToLowerInvariant()}') OR (is_defined(tags.deviceName) AND STARTSWITH(tags.deviceName, '{searchText}')))";
#pragma warning restore CA1308 // Normalize strings to uppercase
            }

            if (searchTags != null)
            {
                var tagsFilterBuilder = new StringBuilder();

                foreach (var item in searchTags)
                {
                    _ = tagsFilterBuilder.Append(CultureInfo.InvariantCulture, $" AND is_defined(tags.{item.Key}) AND STARTSWITH(tags.{item.Key}, '{item.Value}')");
                }

                filter += tagsFilterBuilder;
            }

            if (searchStatus != null)
            {
                filter += $" AND status = '{(searchStatus.Value ? "enabled" : "disabled")}'";
            }

            if (searchState != null)
            {
                filter += $" AND connectionState = '{(searchState.Value ? "Connected" : "Disconnected")}'";
            }

            var emptyResult = new PaginationResult<Twin>
            {
                Items = Enumerable.Empty<Twin>(),
                TotalItems = 0
            };

            var stopWatch = Stopwatch.StartNew();

            IEnumerable<string> count;

            try
            {
                count = await this.registryManager
                    .CreateQuery($"SELECT COUNT() as totalNumber FROM devices {filter}")
                    .GetNextAsJsonAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Unable to get devices count", e);
            }

            this.log.LogDebug($"Count obtained in {stopWatch.Elapsed}");

            if (!JObject.Parse(count.Single()).TryGetValue("totalNumber", out var result))
            {
                return emptyResult;
            }

            if (result.Value<int>() == 0)
            {
                return emptyResult;
            }

            stopWatch.Restart();

            try
            {
                var query = this.registryManager
                    .CreateQuery($"SELECT * FROM devices { filter }", pageSize);

                var response = await query
                    .GetNextAsTwinAsync(new QueryOptions
                    {
                        ContinuationToken = continuationToken
                    });

                this.log.LogDebug($"Data obtained in {stopWatch.Elapsed}");

                return new PaginationResult<Twin>
                {
                    Items = response,
                    TotalItems = result.Value<int>(),
                    NextPage = response.ContinuationToken
                };
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException($"Unable to query devices", e);
            }
        }

        /// <summary>
        /// this function return the device we want with the registry manager.
        /// </summary>
        /// <param name="deviceId">device id.</param>
        /// <returns>Device.</returns>
        public async Task<Device> GetDevice(string deviceId)
        {
            try
            {
                return await this.registryManager.GetDeviceAsync(deviceId);
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException($"Unable to get device with id {deviceId}", e);
            }
        }

        /// <summary>
        /// this function use the registry manager to find the twin
        /// of a device.
        /// </summary>
        /// <param name="deviceId">id of the device.</param>
        /// <returns>Twin of a device.</returns>
        public async Task<Twin> GetDeviceTwin(string deviceId)
        {
            try
            {
                return await this.registryManager.GetTwinAsync(deviceId);
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException($"Unable to get device twin with id {deviceId}", e);
            }
        }

        /// <summary>
        /// this function execute a query with the registry manager to find
        /// the twin of the device with this module.
        /// </summary>
        /// <param name="deviceId">id of the device.</param>
        /// <returns>Twin of the device.</returns>
        public async Task<Twin> GetDeviceTwinWithModule(string deviceId)
        {
            var devicesWithModules = this.registryManager.CreateQuery($"SELECT * FROM devices.modules WHERE devices.modules.moduleId = '$edgeAgent' AND deviceId in ['{deviceId}']");

            while (devicesWithModules.HasMoreResults)
            {
                try
                {
                    var devicesTwins = await devicesWithModules.GetNextAsTwinAsync();
                    return devicesTwins.ElementAt(0);
                }
                catch (Exception e)
                {
                    throw new InternalServerErrorException($"Unable to get devices twins", e);
                }
            }

            return null;
        }

        /// <summary>
        /// This function create a new device with his twin.
        /// </summary>
        /// <param name="deviceId">the device id.</param>
        /// <param name="isEdge">boolean.</param>
        /// <param name="twin">the twin of my new device.</param>
        /// <param name="isEnabled">the status of the device(disabled by default).</param>
        /// <returns>BulkRegistryOperation.</returns>
        public async Task<BulkRegistryOperationResult> CreateDeviceWithTwin(string deviceId, bool isEdge, Twin twin, DeviceStatus isEnabled = DeviceStatus.Disabled)
        {
            var device = new Device(deviceId)
            {
                Capabilities = new DeviceCapabilities { IotEdge = isEdge },
                Status = isEnabled
            };

            try
            {
                return await this.registryManager.AddDeviceWithTwinAsync(device, twin);
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException($"Unable to create the device twin with id {deviceId}: {e.Message}", e);
            }
        }

        /// <summary>
        /// This function delete a device.
        /// </summary>
        /// <param name="deviceId">the device id.</param>
        public async Task DeleteDevice(string deviceId)
        {
            try
            {
                await this.registryManager.RemoveDeviceAsync(deviceId);
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException($"Unable to delete the device with id {deviceId}", e);
            }
        }

        /// <summary>
        /// This function update a device.
        /// </summary>
        /// <param name="device">the device id.</param>
        /// <returns>the updated device.</returns>
        public async Task<Device> UpdateDevice(Device device)
        {
            try
            {
                return await this.registryManager.UpdateDeviceAsync(device);
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException($"Unable to update the device with id {device.Id}", e);
            }
        }

        /// <summary>
        /// This function update the twin of the device.
        /// </summary>
        /// <param name="deviceId">the device id.</param>
        /// <param name="twin">the new twin.</param>
        /// <returns>the updated twin.</returns>
        public async Task<Twin> UpdateDeviceTwin(string deviceId, Twin twin)
        {
            ArgumentNullException.ThrowIfNull(twin, nameof(twin));

            try
            {
                return await this.registryManager.UpdateTwinAsync(deviceId, twin, twin.ETag);
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException($"Unable to update the device twin with id {deviceId}", e);
            }
        }

        /// <summary>
        /// this function execute a methode on the device.
        /// </summary>
        /// <param name="deviceId">the device id.</param>
        /// <param name="method">the cloud to device method.</param>
        /// <returns>CloudToDeviceMethodResult.</returns>
        public async Task<CloudToDeviceMethodResult> ExecuteC2DMethod(string deviceId, CloudToDeviceMethod method)
        {
            try
            {
                return await this.serviceClient.InvokeDeviceMethodAsync(deviceId, "$edgeAgent", method);
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException($"Unable to execute the cloud to device method {method.MethodName} on the device with id {deviceId}", e);
            }
        }

        /// <summary>
        /// Get edge device logs
        /// </summary>
        /// <param name="deviceId">Device Id</param>
        /// <param name="edgeModule">Edge module</param>
        /// <returns>Edge device logs</returns>
        public async Task<IEnumerable<IoTEdgeDeviceLog>> GetEdgeDeviceLogs(string deviceId, IoTEdgeModule edgeModule)
        {
            var method = new CloudToDeviceMethod(CloudToDeviceMethods.GetModuleLogs);

            var logs = new List<IoTEdgeDeviceLog>();

            var payload = JsonConvert.SerializeObject(new
            {
                schemaVersion = edgeModule.Version,
                items = new[]
                {
                    new
                    {
                        id = edgeModule.ModuleName,
                        filter = new
                        {
                            tail = 300
                        }
                    }
                },
                encoding = "none",
                contentType = "json"
            });

            _ = method.SetPayloadJson(payload);

            var result = await ExecuteC2DMethod(deviceId, method);

            if (result.Status == 200)
            {
                var payloadResponse = result.GetPayloadAsJson();

                if (string.IsNullOrWhiteSpace(payloadResponse))
                {
                    this.log.LogInformation($"Payload logs' response of the device {deviceId} is empty");
                }
                else
                {
                    var payloadResponseAsJson = JsonConvert.DeserializeObject<dynamic[]>(payloadResponse).Single().payload.ToString();

                    logs.AddRange(JsonConvert.DeserializeObject<List<IoTEdgeDeviceLog>>(payloadResponseAsJson));
                }
            }
            else
            {
                this.log.LogError($"Unable to retrieve logs of the device {deviceId}, status code: {result.Status}");
            }

            return logs.OrderByDescending(log => log.TimeStamp);
        }
    }
}
