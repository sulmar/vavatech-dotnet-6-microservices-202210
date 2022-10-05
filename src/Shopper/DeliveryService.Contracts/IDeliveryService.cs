using ProtoBuf;
using ProtoBuf.Grpc;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace DeliveryService.Contracts
{

    // dotnet add package protobuf-net.Grpc

    [ServiceContract]
    public interface IDeliveryService
    {
        [OperationContract]
        Task<ConfirmDeliveryResponse> ConfirmDeliveryAsync(ConfirmDeliveryRequest request, CallContext context = default);
    }

    [DataContract]
    public class ConfirmDeliveryRequest
    {
        [DataMember(Order = 1)]
        public int BoxId { get; set; }
        [DataMember(Order = 2)]
        public DateTime ShippedDate { get; set; }
        [DataMember(Order = 3)]
        public string Sign { get; set; }
    }

    [DataContract]
    public class ConfirmDeliveryResponse
    {
        [DataMember(Order = 1)]
        public bool IsConfirmed { get; set; }
    }



    /*
    [ServiceContract]
    public interface IDeliveryService
    {
        Task<ConfirmDeliveryResponse> ConfirmDeliveryAsync(ConfirmDeliveryRequest request, CallContext context = default);
    }

    [ProtoContract]
    public class ConfirmDeliveryRequest
    {
        [ProtoMember( 1)]
        public int BoxId { get; set; }
        [ProtoMember( 2)]
        public DateTime ShippedDate { get; set; }
        [ProtoMember( 3)]
        public string Sign { get; set; }
    }

    [ProtoContract]
    public class ConfirmDeliveryResponse
    {
        [ProtoMember( 1)]
        public bool IsConfirmed { get; set; }
    }
    */
}