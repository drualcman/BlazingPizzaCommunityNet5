using BlazingPizza.ComponentsLibrary.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingPizza.Shared
{
    public class OrderWithStatus
    {
        const string Preparing = "Preparando";
        const string OutForDelivery = "En camino";
        const string Delivered = "Entregado";

        public readonly static TimeSpan PreparationDuration =
            TimeSpan.FromSeconds(10);

        public readonly static TimeSpan DeliveryDuration =
            TimeSpan.FromMinutes(1);

        public Order Order { get; set; }
        public string StatusText { get; set; }
        public bool IsDelivered => StatusText == Delivered;
        public List<Markerasdadsad> MapMarkers { get; set; }
        public static OrderWithStatus FromOrder(Order order)
        {
            string Message;
            List<Markerasdadsad> Markers;
            var DispatchTime = order.CreatedTime.Add(PreparationDuration);

            if (DateTime.Now < DispatchTime)
            {
                Message = Preparing;
                Markers = new List<Markerasdadsad>
                {
                    ToMapMarker("Usted",
                    order.DeliveryLocation, showPopup: true)
                };
            }
            else if (DateTime.Now < DispatchTime + DeliveryDuration)
            {
                Message = OutForDelivery;

                var StartPosition = ComputeStartPosition(order);
                var ProportionOfDeliveryCompleted =
                    Math.Min(1,
                    (DateTime.Now - DispatchTime).TotalMilliseconds
                    / DeliveryDuration.TotalMilliseconds);
                var DriverPosition = LatLong.Interpolate(
                    StartPosition,
                    order.DeliveryLocation,
                    ProportionOfDeliveryCompleted);
                Markers = new List<Markerasdadsad>
                {
                    ToMapMarker("Usted", order.DeliveryLocation),
                    ToMapMarker("Repartidor", DriverPosition, showPopup: true),
                };
            }
            else
            {
                Message = Delivered;
                Markers = new List<Markerasdadsad>
                {
                    ToMapMarker("Ubicación de entrega",
                    order.DeliveryLocation, showPopup: true),
                };
            }

            return new OrderWithStatus
            {
                Order = order,
                StatusText = Message,
                MapMarkers = Markers,
            };
        }

        private static LatLong ComputeStartPosition(Order order)
        {
            var Random = new Random(order.OrderId);
            var Distance = 0.01 + Random.NextDouble() * 0.02;
            var Angle = Random.NextDouble() * Math.PI * 2;
            var Offset =
                (Distance * Math.Cos(Angle),
                Distance * Math.Sin(Angle));
            return new LatLong(
                order.DeliveryLocation.Latitude + Offset.Item1,
                order.DeliveryLocation.Longitude + Offset.Item2);
        }

        static Markerasdadsad ToMapMarker(string description,
            LatLong coords, bool showPopup = false)
            => new Markerasdadsad
            {
                Description = description,
                X = coords.Longitude,
                Y = coords.Latitude,
                ShowPopup = showPopup
            };

    }


}
