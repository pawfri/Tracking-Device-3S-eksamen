using System.Reflection;
using TrackingDeviceLib.Services.Repositories;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace TestREST
{
    [TestClass]
    public sealed class TestREST
    {

        private static readonly TrackingDeviceRepo _repo = new TrackingDeviceRepo();

        [TestCleanup]
        public void Cleanup()
        {
            _repo.GetAll().Clear();
        }

        /// <summary>
        /// Tester at Repository oprettet et objekt
        /// </summary>
        /// <param name="longitude">lon-paramter værdi</param>
        /// <param name="latitude">lat-paramter værdi</param>
        /// <param name="timestampString">Tidspunkt</param>
        [TestMethod]
        [DataRow(12.3456, 65.4321, "2024-05-01T12:00:00Z")]
        [DataRow(10.0000, 20.0000, "2023-12-31T23:59:59Z")]
        public void CreateObject(double longitude, double latitude, string timestampString)
        {
            // Arrange
            var timestamp = DateTime.Parse(timestampString, null, System.Globalization.DateTimeStyles.RoundtripKind);

            // Act
            var model = new Location(longitude, latitude, timestamp);

            // Assert
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual(longitude, model.Longitude);
            Assert.AreEqual(latitude, model.Latitude);
            Assert.AreEqual(timestamp, model.Timestamp);

        }

        /// <summary>
        /// Tester at id er unikt, når der oprettes et nyt objekt
        /// </summary>
        /// <param name="longitude">lon-paramter værdi</param>
        /// <param name="latitude">lat-paramter værdi</param>
        /// <param name="timestampString">Tidspunkt</param>
        [TestMethod]
        [DataRow(12.3456, 65.4321, "2024-05-01T12:00:00Z")]
        public void UniqueId(double longitude, double latitude, string timestampString)
        {
            // Arrange
            var model1 = new Location(longitude, latitude, timestamp);

            // Act
            var model2 = new Location(longitude, latitude, timestamp);

            // Assert
            Assert.AreEqual(1, model1.Id);
            Assert.AreEqual(2, model2.Id);

        }

    }
}
