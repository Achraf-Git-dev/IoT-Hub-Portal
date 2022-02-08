[![Build & Test](https://github.com/CGI-FR/iot-hub-portal/actions/workflows/build.yml/badge.svg)](https://github.com/CGI-FR/iot-hub-portal/actions/workflows/build.yml)
[![Continuous Deployment](https://github.com/CGI-FR/iot-hub-portal/actions/workflows/publish.yml/badge.svg)](https://github.com/CGI-FR/iot-hub-portal/actions/workflows/publish.yml)
[![Deploy Staging](https://github.com/CGI-FR/IoT-Hub-Portal/actions/workflows/deploy_staging.yml/badge.svg)](https://github.com/CGI-FR/IoT-Hub-Portal/actions/workflows/deploy_staging.yml)
[![codecov](https://codecov.io/gh/CGI-FR/IoT-Hub-Portal/branch/main/graph/badge.svg?token=S1A59KMRV6)](https://codecov.io/gh/CGI-FR/IoT-Hub-Portal)

# IoT Hub Portal

This project aims to provide a solution for handling IoT Devices easyly.
It leverages on Azure IoT Hub for connectivity and device management.

![https://cgi-fr.github.io/IoT-Hub-Portal/images/architecture.png](https://cgi-fr.github.io/IoT-Hub-Portal/images/architecture.png)

## Features

* Portal Authentication
* IoT Device & device Model management
* IoT Edge device management
* C2D Methods
* LoRA WAN device connectivity

## Prerequisites

The following should be completed before proceeding with the IoT Hub Portal development or deployment in your environment.

* You must have an Azure subscription. Get an [Azure Free account](https://azure.microsoft.com/en-us/offers/ms-azr-0044p/) to get started.
* You must have configured an Azure AD B2C Tenant with applications. See [Portal AD applications configuration](./b2c-applications.md) page.
* Understandr how IoTEdge LoraWAN StarterKit work. Have a look at [https://azure.github.io/iotedge-lorawan-starterkit](https://azure.github.io/iotedge-lorawan-starterkit) to get started.

## Quick Start

### Deployed Azure Resources

The template will deploy in your Azure subscription the Following resources:

* IoT Hub
* Azure Function and Consumption Service Plan
* Redis Cache
* Application Insights
* Log Analytics (when opted in to use Azure Monitor)
* Azure WebApp and Service Plan

### Instructions 

1. Choose a solution prefix for your deployment.
1. Use [Portal AD applications configuration](https://cgi-fr.github.io/IoT-Hub-Portal/docs/b2c-applications.html) page to configure your AD B2C Tenant.
    > You should have recorded the following information:
    > * OpenID authority: `<your-openid-authority>`
    > * OpenID metadata URL: `<your-openid-provider-metadata-url>`
    > * Client ID: `<your-client-id>`
    > * API Client ID: `<your-client-id>`

1. Press on the button here below to start your Azure Deployment.

    [![Deploy](http://azuredeploy.net/deploybutton.png)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FCGI-FR%2FIoT-Hub-Portal%2Fmain%2Ftemplates%2Fazuredeploy.json)

1. You will get to a page asking you to fill the following fields :
    * **Resource Group**: A logical "folder" where all the template resource would be put into, just choose a meaningful name. 
    * **Location**: In which DataCenter the resources should be deployed. Make sure to choose a location where IoT Hub is available
    * **Unique Solution Prefix**: A string that would be used as prefix for all the resources name to ensure their uniqueness.
    * **Open Id Authority**: The OpenID authority used by the portal.
    * **OpenId Metadata URL**: The OpenID metadata URL used by the portal.
    * **Client Id**: the ID of the web client that will be used to authenticate the portal.
    * **Api Client Id**: the ID of the API client that will be used to authenticate the portal.
    * **Edge gateway name**: the name of your LoRa Gateway node in the IoT Hub.
    * **Deploy Device**: Do you want demo end devices to be already provisioned (one using OTAA and one using ABP)? If yes set this to true, the code located in the Arduino folder would be ready to use immediately.
    * **Reset pin**:  The reset pin of your gateway (the value should be 7 for the Seed Studio LoRaWan, 25 for the IC880A)
    * **Region**:  In what region are you operating your device (currently only EU868 and US915 is supported)

    > see: [https://azure.github.io/iotedge-lorawan-starterkit/dev/quickstart/#deployed-azure-infrastructure](https://azure.github.io/iotedge-lorawan-starterkit/dev/quickstart/#deployed-azure-infrastructure) for more information about the LoRaWan IoT Hub and Azure deployment.

## Screenshots

You can find below some screenshot that can help you to understand the possibilities of this solution.

### Device list

![image](https://user-images.githubusercontent.com/9513635/153055836-11bd5c30-2efc-4d43-95e9-5e07e0e2e6cc.png)

### Device details

![image](https://user-images.githubusercontent.com/9513635/153073182-aec2e029-272a-4a8a-b713-424308d51c81.png)

### Device models

![image](https://user-images.githubusercontent.com/9513635/153073065-0fa9b282-c722-464b-93f8-c412812c5ada.png)

## Documentation

Our documentation is present at github page: [https://cgi-fr.github.io/IoT-Hub-Portal/](https://cgi-fr.github.io/IoT-Hub-Portal/).

## Known Issues and Limitations

Refer to [Known Issues](knownissues) for known issues, gotchas and limitations.

## Support

This is an open source solution.
For bugs and issues with the codebase please log an issue in this repo.

## Credits

* [Azure IoT Edge LoRaWAN Starter Kit](https://github.com/Azure/iotedge-lorawan-starterkit)
