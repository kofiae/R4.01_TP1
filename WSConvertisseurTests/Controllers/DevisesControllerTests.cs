using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSConvertisseur.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WSConvertisseur.Models;
using Microsoft.AspNetCore.Http;

namespace WSConvertisseur.Controllers.Tests
{
    [TestClass()]
    public class DevisesControllerTests
    {
        [TestInitialize]
        public void InitialisationDesTests()
        {
            DevisesController controller = new DevisesController();
        }

        [TestCleanup]
        public void NettoyageDesTests()
        {
            // Nettoyer les variables, ... après chaque test
        }

        [TestMethod]
        public void GetAll_ReturnsRightItems()
        {
            //Arrange
            DevisesController controller = new DevisesController();

            // Act
            var result = controller.GetAll();

            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Devise>), "Pas un IEnumerable"); // Test du type de retour
           

        }

        [TestMethod]
        public void GetById_ExistingIdPassed_ReturnsRightItem()
        {
            // Arrange
            DevisesController controller = new DevisesController();

            // Act
            var result = controller.GetById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult"); // Test du type de retour
            Assert.IsNull(result.Result, "Erreur est pas null"); //Test de l'erreur
            Assert.IsInstanceOfType(result.Value, typeof(Devise), "Pas une Devise"); // Test du type du contenu (valeur) du retour
            Assert.AreEqual(new Devise(1, "Dollar", 1.08), (Devise?)result.Value, "Devises pas identiques"); //Test de la devise récupérée
          }

        [TestMethod]
        public void GetById_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            //Arrange
            DevisesController controller = new DevisesController();

            //Act
            var result = controller.GetById(10);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult"); // Test du type de retour
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult), "L'erreur doit être NotFound"); // Test du type de l'erreur
            Assert.IsNull(result.Value, "Pas une Devise"); // Test du type du contenu (valeur) du retour
        }

        [TestMethod]
        public void Post_InvalidObjectPassed_ReturnsNotFoundResult()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            Devise d = new Devise(4, "yen", 1.5);

            // Act
            var result = controller.Post(d);
            CreatedAtRouteResult routeResult = (CreatedAtRouteResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult"); // Test du type de result
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtRouteResult), "Pas un CreatedAtRouteResult"); // Test du type de result.Result
            
            Assert.AreEqual(routeResult.StatusCode, StatusCodes.Status201Created, "Les status codes ne sont pas egaux"); // Test des status code
            Assert.AreEqual(new Devise(4, "yen", 1.5), (Devise?)routeResult.Value, ""); // Test de la devise stocké
        }

        /* Pas testable
        [TestMethod]
        public void Post_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            Devise devise = new Devise(4, null, 4.0);

            // Act
            var result = controller.Post(devise);
            CreatedAtRouteResult routeResult = (CreatedAtRouteResult)result.Result;

            //Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult"); // Test du type de result
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtRouteResult), "Pas un CreatedAtRouteResult"); // Test du type de result.Result

            Assert.AreEqual(routeResult.StatusCode, StatusCodes.Status201Created, "Les status codes ne sont pas egaux"); // Test des status code
            Assert.AreEqual(new Devise(4, "yen", 1.5), (Devise?)routeResult.Value, ""); // Test de la devise stocké
        }
        */


        [TestMethod]
        public void Put_Invalid_Update_ReturnsBadRequest()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            Devise devise = new Devise(1, "Dollar", 4.0);
            int id = 4;

            // Act
            var result = controller.Put(id, devise);

            //Assert
            Assert.AreNotEqual(id, devise.Id);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult), "L'erreur doit être BadRequest"); // Test du type de l'erreur

        }

        [TestMethod]
        public void Put_Invalid_Update_ReturnsNotFound()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            Devise devise = new Devise(5, "Dollar", 4.0);
            int id = 5;
            //Act
            var result = controller.Put(id, devise);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult), "L'erreur doit être NotFound"); // Test du type de l'erreur
       
        }

        [TestMethod]
        public void Put_Valid_Update_ReturnsNoContent()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            Devise devise = new Devise(1, "Dollar", 4.0);
            int id = 1;

            //Act
            var result = controller.Put(id, devise);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "L'erreur doit être NotFound"); // Test du type de l'erreur

        }

        [TestMethod]
        public void Delete_NotOK_ReturnsNotFound()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            int id = 4;

            //Act
            var result = controller.Delete(id);

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult), "L'erreur doit être NotFound"); // Test du type de l'erreur

        }

        [TestMethod]
        public void Delete_OK_ReturnsRightItem()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            Devise devise = new Devise(1, "Dollar", 1.08);
            int id = 1;

            //Act
            var result = controller.Delete(id);

            //Assert
            Assert.AreEqual(result.Value, devise, "La devise doit être retourné"); // Test le retour
        }
    }
}