using FluentValidation.TestHelper;
using MicroserviceBarebone.Application.Vehiculs.Commands.CreateVehicul;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MicroserviceBarebone.Application.Tests.Vehiculs.Commands
{
    public class CreateVehiculCommandValidatorTests
    {
        [Fact]
        public void Shoud_have_error_if_vehiculId_is_null()
        {
            #region Arrange
            var validator = new CreateVehiculCommandValidator();
            #endregion

            #region Act & Assert
            validator.ShouldHaveValidationErrorFor(vehicul => vehicul.VehiculId, Guid.Empty);
            #endregion

        }

        [Fact]
        public void Shoud_not_have_error_if_vehiculId_is_specified()
        {
            #region Arrange
            var validator = new CreateVehiculCommandValidator();
            #endregion

            #region Act & Assert
            validator.ShouldNotHaveValidationErrorFor(vehicul => vehicul.VehiculId, Guid.NewGuid());
            #endregion

        }

        [Fact]
        public void Shoud_have_error_if_name_greater_than_255()
        {
            #region Arrange
            var validator = new CreateVehiculCommandValidator();
            #endregion

            #region Act & Assert
            validator.ShouldHaveValidationErrorFor(vehicul => vehicul.Label, new string('*', 256));
            #endregion
        }

        [Fact]
        public void Shoud_not_have_error_if_name_greater_than_255()
        {
            #region Arrange
            var validator = new CreateVehiculCommandValidator();
            #endregion

            #region Act & Assert
            validator.ShouldNotHaveValidationErrorFor(vehicul => vehicul.Label, new string('*', 255));
            #endregion
        }
    }
}
