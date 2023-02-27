// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AzureIoTHub.Portal.Client.Validators
{
    using AzureIoTHub.Portal.Models.v10;
    using FluentValidation;

    public class DeviceModelValidator : AbstractValidator<DeviceModelDto>
    {
        public DeviceModelValidator()
        {
            _ = RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Model name is required.");
        }
    }
}
