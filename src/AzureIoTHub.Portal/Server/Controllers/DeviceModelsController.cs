﻿// Copyright (c) CGI France - Grand Est. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AzureIoTHub.Portal.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Azure.Data.Tables;
    using AzureIoTHub.Portal.Server.Factories;
    using AzureIoTHub.Portal.Server.Managers;
    using AzureIoTHub.Portal.Server.Mappers;
    using AzureIoTHub.Portal.Shared.Models;
    using AzureIoTHub.Portal.Shared.Security;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleNames.Admin)]
    public class DeviceModelsController : ControllerBase
    {
        private const string DefaultPartitionKey = "0";

        private readonly ITableClientFactory tableClientFactory;
        private readonly IDeviceModelMapper deviceModelMapper;
        private readonly IDeviceModelCommandMapper deviceModelCommandMapper;
        private readonly IDeviceModelImageManager deviceModelImageManager;

        public DeviceModelsController(
            IDeviceModelImageManager deviceModelImageManager,
            IDeviceModelCommandMapper deviceModelCommandMapper,
            IDeviceModelMapper deviceModelMapper,
            ITableClientFactory tableClientFactory)
        {
            this.deviceModelMapper = deviceModelMapper;
            this.tableClientFactory = tableClientFactory;
            this.deviceModelCommandMapper = deviceModelCommandMapper;
            this.deviceModelImageManager = deviceModelImageManager;
        }

        /// <summary>
        /// Gets a list of device models from an Azure DataTable.
        /// </summary>
        /// <returns>A list of DeviceModel.</returns>
        [HttpGet]
        public IEnumerable<DeviceModel> Get()
        {
            // PartitionKey 0 contains all device models
            var entities = this.tableClientFactory
                            .GetDeviceTemplates()
                            .Query<TableEntity>();

            // Converts the query result into a list of device models
            var deviceModelList = entities.Select(this.deviceModelMapper.CreateDeviceModel);

            return deviceModelList;
        }

        /// <summary>
        /// Gets a list of device models from an Azure DataTable.
        /// </summary>
        /// <returns>A list of DeviceModel.</returns>
        [HttpGet("{modelID}")]
        public IActionResult Get(string modelID)
        {
            var query = this.tableClientFactory
                            .GetDeviceTemplates()
                            .Query<TableEntity>(t => t.RowKey == modelID);

            if (!query.Any())
            {
                return this.NotFound();
            }

            return this.Ok(this.deviceModelMapper.CreateDeviceModel(query.Single()));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string deviceModel, [FromForm] IFormFile file = null)
        {
            try
            {
                DeviceModel deviceModelObject = JsonConvert.DeserializeObject<DeviceModel>(deviceModel);
                TableEntity entity = new TableEntity()
                {
                    PartitionKey = DefaultPartitionKey,
                    RowKey = Guid.NewGuid().ToString()
                };

                await this.SaveEntity(entity, deviceModelObject, file);

                return this.Ok();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm] string deviceModel, [FromForm] IFormFile file = null)
        {
            try
            {
                DeviceModel deviceModelObject = JsonConvert.DeserializeObject<DeviceModel>(deviceModel);

                TableEntity entity = new TableEntity()
                {
                    PartitionKey = DefaultPartitionKey,
                    RowKey = deviceModelObject.ModelId
                };

                await this.SaveEntity(entity, deviceModelObject, file);

                return this.Ok();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [HttpDelete("{deviceModelID}")]
        public async Task<IActionResult> Delete(string deviceModelID)
        {
            var result = await this.tableClientFactory
                .GetDeviceTemplates()
                .DeleteEntityAsync(DefaultPartitionKey, deviceModelID);

            return this.StatusCode(result.Status);
        }

        private async Task SaveEntity(TableEntity entity, DeviceModel deviceModelObject, [FromForm] IFormFile file = null)
        {
            this.deviceModelMapper.UpdateTableEntity(entity, deviceModelObject);

            await this.tableClientFactory
                .GetDeviceTemplates()
                .UpsertEntityAsync(entity);

            if (file != null)
            {
                using var fileStream = file.OpenReadStream();
                await this.deviceModelImageManager.ChangeDeviceModelImageAsync(entity.RowKey, fileStream);
            }

            // insertion des commant
            if (deviceModelObject.Commands.Count > 0)
            {
                foreach (var element in deviceModelObject.Commands)
                {
                    TableEntity commandEntity = new TableEntity()
                    {
                        PartitionKey = deviceModelObject.Name,
                        RowKey = element.Name
                    };

                    this.deviceModelCommandMapper.UpdateTableEntity(commandEntity, element);

                    await this.tableClientFactory
                        .GetDeviceCommands()
                        .AddEntityAsync(commandEntity);
                }
            }
        }
    }
}
