// File Name : DriverValidation.cs
// NUnit test for user inputs of driver model
//
// Author : Sam Sangkyun Park
// Date Created : Nov. 17, 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using A4BusService.Models;
using System.Data.Entity;

namespace A4BusService.Tests.Controllers
{
    [TestFixture]
    public class DriverValidation
    {
        #region [SetUp] and [tearDown] methods - before & after each test

        driver driver;
        BusServiceContext db = new BusServiceContext();
        Random rand = new Random();

        [SetUp]
        public void Setup()
        {
            driver = new driver();
            driver.driverId = rand.Next();
            driver.firstName = "FIRSTNAME";
            driver.lastName = "LASTNAME";
            driver.homePhone = "1234567890";
            driver.postalCode = "A3A 3A3";
            driver.provinceCode = "ON";
            driver.dateHired = DateTime.Parse("2015-11-13");
        }

        [TearDown]
        public void Cleanup()
        {
            db.Entry(driver).State = EntityState.Detached;
        }

        #endregion

        [Test]
        public void driverValidation_UpperCasePostalWithSpace_shouldPass()
        {
            // Arrange
            driver.postalCode = "A3A 3A3";

            // Act
            try
            {
                db.drivers.Add(driver);
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Assert.Fail("got an edit error on 'A3A 3A3'");
            }
            catch (Exception ex)
            {
                Assert.Fail("got and undefined exception: " + ex.GetBaseException().Message);
            }

            // Assert
            Assert.AreEqual("A3A 3A3", driver.postalCode);
        }

        [Test]
        public void driverValidation_UpperCasePostalNoSpace_shouldPassAndReformat()
        {
            // Arrange
            driver.postalCode = "A3A3A3";

            // Act
            try
            {
                db.drivers.Add(driver);
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Assert.Fail("got an edit error on 'A3A3A3'");
            }
            catch (Exception ex)
            {
                Assert.Fail("got and undefined exception: " + ex.GetBaseException().Message);
            }

            // Assert
            Assert.AreEqual("A3A 3A3", driver.postalCode);
        }

        [Test]
        public void driverValidation_LowerCasePostalWithSpace_shouldPassAndReformat()
        {
            // Arrange
            driver.postalCode = "a3a 3a3";

            // Act
            try
            {
                db.drivers.Add(driver);
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Assert.Fail("got an edit error on 'a3a 3a3'");
            }
            catch (Exception ex)
            {
                Assert.Fail("got and undefined exception: " + ex.GetBaseException().Message);
            }

            // Assert
            Assert.AreEqual("A3A 3A3", driver.postalCode);
        }

        [Test]
        public void driverValidation_LowerCasePostalNoSpace_shouldPassAndReformat()
        {
            // Arrange
            driver.postalCode = "a3a3a3";

            // Act
            try
            {
                db.drivers.Add(driver);
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Assert.Fail("got an edit error on 'a3a3a3'");
            }
            catch (Exception ex)
            {
                Assert.Fail("got and undefined exception: " + ex.GetBaseException().Message);
            }

            // Assert
            Assert.AreEqual("A3A 3A3", driver.postalCode);
        }

        [Test]
        public void driverValidation_UnmatchedPostalCodeFormat_shouldFail()
        {
            // Arrange
            driver.postalCode = "D3A 3A3";

            // Act
            try
            {
                db.drivers.Add(driver);
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
	            {
		            sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors) 
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
	            }
                Assert.Fail("got an edit error on 'D3A 3A3'" + sb.ToString());
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                Assert.Fail("got and undefined exception: " + ex.GetBaseException().Message);
            }

            // Assert
            Assert.AreEqual("A3A 3A3", driver.postalCode);
        }

        [Test]
        public void driverValidation_noSpacePostalCodeFormat_shouldPassAndReformat()
        {
            // Arrange
            driver.postalCode = "A3A3A3";

            // Act
            try
            {
                db.drivers.Add(driver);
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Assert.Fail("got an edit error on 'A3A3A3'");
            }
            catch (Exception ex)
            {
                Assert.Fail("got and undefined exception: " + ex.GetBaseException().Message);
            }

            // Assert
            Assert.AreEqual("A3A 3A3", driver.postalCode);
        }
    }
}
