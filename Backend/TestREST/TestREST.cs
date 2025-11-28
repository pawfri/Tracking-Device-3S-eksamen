using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Threading;
using TrackingDeviceLib;
using TrackingDeviceLib.Services.Repositories;
//using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace TestREST
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public sealed class TestREST
    {

        private static readonly TrackingDeviceRepo _repo = new TrackingDeviceRepo();

        /// <summary>
        /// Tester at Repository oprettet et objekt
        /// </summary>
        /// <param name="longitude">lon-paramter værdi</param>
        /// <param name="latitude">lat-paramter værdi</param>
        /// <param name="timestampString">Tidspunkt</param>
        [TestMethod]
        [DataRow(1, 12.3456, 65.4321, "2024-05-01T12:00:00Z")]
        public void CreateObject(int id, double longitude, double latitude, string timestampString)
        {
            // Arrange
            var date = DateTime.Parse(timestampString, null, System.Globalization.DateTimeStyles.RoundtripKind);
            
            // Act
            var model = new Location(longitude, latitude, date);
            _repo.Add(model);

            // Assert
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual(longitude, model.Longitude);
            Assert.AreEqual(latitude, model.Latitude);
            Assert.AreEqual(date, model.Date);
        }

        /// <summary>
        /// Tester at id er unikt, når der oprettes et nyt objekt
        /// </summary>
        /// <param name="longitude">lon-paramter værdi</param>
        /// <param name="latitude">lat-paramter værdi</param>
        /// <param name="timestampString">Tidspunkt</param>
        ///
        [TestMethod]
  //      [DataRow(0, 12.3456, 65.4321, "2024-05-01T12:00:00Z")]
		//[DataRow(1, 12.3456, 65.4321, "2024-05-01T12:00:00Z")]
		public void UniqueId()
        {
			// Arrange
			var repo = new TrackingDeviceRepo();
			var date = DateTime.Parse("2024-05-01T12:00:00Z", null, DateTimeStyles.RoundtripKind);

			var model1 = new Location(12.3456, 65.4321, date);
			var model2 = new Location(12.3456, 65.4321, date);

			// Act
			repo.Add(model1);
			repo.Add(model2);

			// Assert
			Assert.AreEqual(1, model1.Id);
			Assert.AreEqual(2, model2.Id);

		}

    }
}
