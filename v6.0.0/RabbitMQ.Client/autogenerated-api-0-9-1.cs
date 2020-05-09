using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Framing.Impl;
using RabbitMQ.Client.Impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Client.Framing
{
    internal sealed class Protocol : RabbitMQ.Client.Framing.Impl.ProtocolBase
    {
        ///<summary>Protocol major version (= 0)</summary>
        public override int MajorVersion => 0;

        ///<summary>Protocol minor version (= 9)</summary>
        public override int MinorVersion => 9;

        ///<summary>Protocol revision (= 1)</summary>
        public override int Revision => 1;

        ///<summary>Protocol API name (= :AMQP_0_9_1)</summary>
        public override string ApiName => ":AMQP_0_9_1";

        ///<summary>Default TCP port (= 5672)</summary>
        public override int DefaultPort => 5672;

        internal override Client.Impl.MethodBase DecodeMethodFrom(ReadOnlyMemory<byte> memory)
        {
            ushort classId = Util.NetworkOrderDeserializer.ReadUInt16(memory);
            ushort methodId = Util.NetworkOrderDeserializer.ReadUInt16(memory.Slice(2));
            Client.Impl.MethodBase result = DecodeMethodFrom(classId, methodId);
            if (result != null)
            {
                Client.Impl.MethodArgumentReader reader = new Client.Impl.MethodArgumentReader(memory.Slice(4));
                result.ReadArgumentsFrom(ref reader);
                return result;
            }

            throw new Client.Exceptions.UnknownClassOrMethodException(classId, methodId);
        }

        internal Client.Impl.MethodBase DecodeMethodFrom(ushort classId, ushort methodId)
        {
            switch ((classId << 16) | methodId)
            {
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.Start: return new Impl.ConnectionStart();
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.StartOk: return new Impl.ConnectionStartOk();
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.Secure: return new Impl.ConnectionSecure();
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.SecureOk: return new Impl.ConnectionSecureOk();
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.Tune: return new Impl.ConnectionTune();
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.TuneOk: return new Impl.ConnectionTuneOk();
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.Open: return new Impl.ConnectionOpen();
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.OpenOk: return new Impl.ConnectionOpenOk();
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.Close: return new Impl.ConnectionClose();
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.CloseOk: return new Impl.ConnectionCloseOk();
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.Blocked: return new Impl.ConnectionBlocked();
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.Unblocked: return new Impl.ConnectionUnblocked();
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.UpdateSecret: return new Impl.ConnectionUpdateSecret();
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.UpdateSecretOk: return new Impl.ConnectionUpdateSecretOk();
                case (ClassConstants.Channel << 16) | ChannelMethodConstants.Open: return new Impl.ChannelOpen();
                case (ClassConstants.Channel << 16) | ChannelMethodConstants.OpenOk: return new Impl.ChannelOpenOk();
                case (ClassConstants.Channel << 16) | ChannelMethodConstants.Flow: return new Impl.ChannelFlow();
                case (ClassConstants.Channel << 16) | ChannelMethodConstants.FlowOk: return new Impl.ChannelFlowOk();
                case (ClassConstants.Channel << 16) | ChannelMethodConstants.Close: return new Impl.ChannelClose();
                case (ClassConstants.Channel << 16) | ChannelMethodConstants.CloseOk: return new Impl.ChannelCloseOk();
                case (ClassConstants.Exchange << 16) | ExchangeMethodConstants.Declare: return new Impl.ExchangeDeclare();
                case (ClassConstants.Exchange << 16) | ExchangeMethodConstants.DeclareOk: return new Impl.ExchangeDeclareOk();
                case (ClassConstants.Exchange << 16) | ExchangeMethodConstants.Delete: return new Impl.ExchangeDelete();
                case (ClassConstants.Exchange << 16) | ExchangeMethodConstants.DeleteOk: return new Impl.ExchangeDeleteOk();
                case (ClassConstants.Exchange << 16) | ExchangeMethodConstants.Bind: return new Impl.ExchangeBind();
                case (ClassConstants.Exchange << 16) | ExchangeMethodConstants.BindOk: return new Impl.ExchangeBindOk();
                case (ClassConstants.Exchange << 16) | ExchangeMethodConstants.Unbind: return new Impl.ExchangeUnbind();
                case (ClassConstants.Exchange << 16) | ExchangeMethodConstants.UnbindOk: return new Impl.ExchangeUnbindOk();
                case (ClassConstants.Queue << 16) | QueueMethodConstants.Declare: return new Impl.QueueDeclare();
                case (ClassConstants.Queue << 16) | QueueMethodConstants.DeclareOk: return new Impl.QueueDeclareOk();
                case (ClassConstants.Queue << 16) | QueueMethodConstants.Bind: return new Impl.QueueBind();
                case (ClassConstants.Queue << 16) | QueueMethodConstants.BindOk: return new Impl.QueueBindOk();
                case (ClassConstants.Queue << 16) | QueueMethodConstants.Unbind: return new Impl.QueueUnbind();
                case (ClassConstants.Queue << 16) | QueueMethodConstants.UnbindOk: return new Impl.QueueUnbindOk();
                case (ClassConstants.Queue << 16) | QueueMethodConstants.Purge: return new Impl.QueuePurge();
                case (ClassConstants.Queue << 16) | QueueMethodConstants.PurgeOk: return new Impl.QueuePurgeOk();
                case (ClassConstants.Queue << 16) | QueueMethodConstants.Delete: return new Impl.QueueDelete();
                case (ClassConstants.Queue << 16) | QueueMethodConstants.DeleteOk: return new Impl.QueueDeleteOk();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Qos: return new Impl.BasicQos();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.QosOk: return new Impl.BasicQosOk();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Consume: return new Impl.BasicConsume();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.ConsumeOk: return new Impl.BasicConsumeOk();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Cancel: return new Impl.BasicCancel();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.CancelOk: return new Impl.BasicCancelOk();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Publish: return new Impl.BasicPublish();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Return: return new Impl.BasicReturn();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Deliver: return new Impl.BasicDeliver();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Get: return new Impl.BasicGet();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.GetOk: return new Impl.BasicGetOk();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.GetEmpty: return new Impl.BasicGetEmpty();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Ack: return new Impl.BasicAck();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Reject: return new Impl.BasicReject();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.RecoverAsync: return new Impl.BasicRecoverAsync();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Recover: return new Impl.BasicRecover();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.RecoverOk: return new Impl.BasicRecoverOk();
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Nack: return new Impl.BasicNack();
                case (ClassConstants.Tx << 16) | TxMethodConstants.Select: return new Impl.TxSelect();
                case (ClassConstants.Tx << 16) | TxMethodConstants.SelectOk: return new Impl.TxSelectOk();
                case (ClassConstants.Tx << 16) | TxMethodConstants.Commit: return new Impl.TxCommit();
                case (ClassConstants.Tx << 16) | TxMethodConstants.CommitOk: return new Impl.TxCommitOk();
                case (ClassConstants.Tx << 16) | TxMethodConstants.Rollback: return new Impl.TxRollback();
                case (ClassConstants.Tx << 16) | TxMethodConstants.RollbackOk: return new Impl.TxRollbackOk();
                case (ClassConstants.Confirm << 16) | ConfirmMethodConstants.Select: return new Impl.ConfirmSelect();
                case (ClassConstants.Confirm << 16) | ConfirmMethodConstants.SelectOk: return new Impl.ConfirmSelectOk();
                default: return null;
            }
        }


        internal override Client.Impl.ContentHeaderBase DecodeContentHeaderFrom(ushort classId)
        {
            switch (classId)
            {
                case 60: return new BasicProperties();
                default: throw new Client.Exceptions.UnknownClassOrMethodException(classId, 0);
            }
        }
    }
    internal sealed class Constants
    {
        ///<summary>(= 1)</summary>
        public const int FrameMethod = 1;
        ///<summary>(= 2)</summary>
        public const int FrameHeader = 2;
        ///<summary>(= 3)</summary>
        public const int FrameBody = 3;
        ///<summary>(= 8)</summary>
        public const int FrameHeartbeat = 8;
        ///<summary>(= 4096)</summary>
        public const int FrameMinSize = 4096;
        ///<summary>(= 206)</summary>
        public const int FrameEnd = 206;
        ///<summary>(= 200)</summary>
        public const int ReplySuccess = 200;
        ///<summary>(= 311)</summary>
        public const int ContentTooLarge = 311;
        ///<summary>(= 313)</summary>
        public const int NoConsumers = 313;
        ///<summary>(= 320)</summary>
        public const int ConnectionForced = 320;
        ///<summary>(= 402)</summary>
        public const int InvalidPath = 402;
        ///<summary>(= 403)</summary>
        public const int AccessRefused = 403;
        ///<summary>(= 404)</summary>
        public const int NotFound = 404;
        ///<summary>(= 405)</summary>
        public const int ResourceLocked = 405;
        ///<summary>(= 406)</summary>
        public const int PreconditionFailed = 406;
        ///<summary>(= 501)</summary>
        public const int FrameError = 501;
        ///<summary>(= 502)</summary>
        public const int SyntaxError = 502;
        ///<summary>(= 503)</summary>
        public const int CommandInvalid = 503;
        ///<summary>(= 504)</summary>
        public const int ChannelError = 504;
        ///<summary>(= 505)</summary>
        public const int UnexpectedFrame = 505;
        ///<summary>(= 506)</summary>
        public const int ResourceError = 506;
        ///<summary>(= 530)</summary>
        public const int NotAllowed = 530;
        ///<summary>(= 540)</summary>
        public const int NotImplemented = 540;
        ///<summary>(= 541)</summary>
        public const int InternalError = 541;
    }
    /// <summary>Autogenerated type. AMQP specification method "connection.start".</summary>
    internal interface IConnectionStart : IMethod
    {
        byte VersionMajor { get; }
        byte VersionMinor { get; }
        IDictionary<string, object> ServerProperties { get; }
        byte[] Mechanisms { get; }
        byte[] Locales { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "connection.start-ok".</summary>
    internal interface IConnectionStartOk : IMethod
    {
        IDictionary<string, object> ClientProperties { get; }
        string Mechanism { get; }
        byte[] Response { get; }
        string Locale { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "connection.secure".</summary>
    internal interface IConnectionSecure : IMethod
    {
        byte[] Challenge { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "connection.secure-ok".</summary>
    internal interface IConnectionSecureOk : IMethod
    {
        byte[] Response { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "connection.tune".</summary>
    internal interface IConnectionTune : IMethod
    {
        ushort ChannelMax { get; }
        uint FrameMax { get; }
        ushort Heartbeat { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "connection.tune-ok".</summary>
    internal interface IConnectionTuneOk : IMethod
    {
        ushort ChannelMax { get; }
        uint FrameMax { get; }
        ushort Heartbeat { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "connection.open".</summary>
    internal interface IConnectionOpen : IMethod
    {
        string VirtualHost { get; }
        string Reserved1 { get; }
        bool Reserved2 { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "connection.open-ok".</summary>
    internal interface IConnectionOpenOk : IMethod
    {
        string Reserved1 { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "connection.close".</summary>
    internal interface IConnectionClose : IMethod
    {
        ushort ReplyCode { get; }
        string ReplyText { get; }
        ushort ClassId { get; }
        ushort MethodId { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "connection.close-ok".</summary>
    internal interface IConnectionCloseOk : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "connection.blocked".</summary>
    internal interface IConnectionBlocked : IMethod
    {
        string Reason { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "connection.unblocked".</summary>
    internal interface IConnectionUnblocked : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "connection.update-secret".</summary>
    internal interface IConnectionUpdateSecret : IMethod
    {
        byte[] NewSecret { get; }
        string Reason { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "connection.update-secret-ok".</summary>
    internal interface IConnectionUpdateSecretOk : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "channel.open".</summary>
    internal interface IChannelOpen : IMethod
    {
        string Reserved1 { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "channel.open-ok".</summary>
    internal interface IChannelOpenOk : IMethod
    {
        byte[] Reserved1 { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "channel.flow".</summary>
    internal interface IChannelFlow : IMethod
    {
        bool Active { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "channel.flow-ok".</summary>
    internal interface IChannelFlowOk : IMethod
    {
        bool Active { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "channel.close".</summary>
    internal interface IChannelClose : IMethod
    {
        ushort ReplyCode { get; }
        string ReplyText { get; }
        ushort ClassId { get; }
        ushort MethodId { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "channel.close-ok".</summary>
    internal interface IChannelCloseOk : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "exchange.declare".</summary>
    internal interface IExchangeDeclare : IMethod
    {
        ushort Reserved1 { get; }
        string Exchange { get; }
        string Type { get; }
        bool Passive { get; }
        bool Durable { get; }
        bool AutoDelete { get; }
        bool Internal { get; }
        bool Nowait { get; }
        IDictionary<string, object> Arguments { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "exchange.declare-ok".</summary>
    internal interface IExchangeDeclareOk : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "exchange.delete".</summary>
    internal interface IExchangeDelete : IMethod
    {
        ushort Reserved1 { get; }
        string Exchange { get; }
        bool IfUnused { get; }
        bool Nowait { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "exchange.delete-ok".</summary>
    internal interface IExchangeDeleteOk : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "exchange.bind".</summary>
    internal interface IExchangeBind : IMethod
    {
        ushort Reserved1 { get; }
        string Destination { get; }
        string Source { get; }
        string RoutingKey { get; }
        bool Nowait { get; }
        IDictionary<string, object> Arguments { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "exchange.bind-ok".</summary>
    internal interface IExchangeBindOk : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "exchange.unbind".</summary>
    internal interface IExchangeUnbind : IMethod
    {
        ushort Reserved1 { get; }
        string Destination { get; }
        string Source { get; }
        string RoutingKey { get; }
        bool Nowait { get; }
        IDictionary<string, object> Arguments { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "exchange.unbind-ok".</summary>
    internal interface IExchangeUnbindOk : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "queue.declare".</summary>
    internal interface IQueueDeclare : IMethod
    {
        ushort Reserved1 { get; }
        string Queue { get; }
        bool Passive { get; }
        bool Durable { get; }
        bool Exclusive { get; }
        bool AutoDelete { get; }
        bool Nowait { get; }
        IDictionary<string, object> Arguments { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "queue.declare-ok".</summary>
    internal interface IQueueDeclareOk : IMethod
    {
        string Queue { get; }
        uint MessageCount { get; }
        uint ConsumerCount { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "queue.bind".</summary>
    internal interface IQueueBind : IMethod
    {
        ushort Reserved1 { get; }
        string Queue { get; }
        string Exchange { get; }
        string RoutingKey { get; }
        bool Nowait { get; }
        IDictionary<string, object> Arguments { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "queue.bind-ok".</summary>
    internal interface IQueueBindOk : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "queue.unbind".</summary>
    internal interface IQueueUnbind : IMethod
    {
        ushort Reserved1 { get; }
        string Queue { get; }
        string Exchange { get; }
        string RoutingKey { get; }
        IDictionary<string, object> Arguments { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "queue.unbind-ok".</summary>
    internal interface IQueueUnbindOk : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "queue.purge".</summary>
    internal interface IQueuePurge : IMethod
    {
        ushort Reserved1 { get; }
        string Queue { get; }
        bool Nowait { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "queue.purge-ok".</summary>
    internal interface IQueuePurgeOk : IMethod
    {
        uint MessageCount { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "queue.delete".</summary>
    internal interface IQueueDelete : IMethod
    {
        ushort Reserved1 { get; }
        string Queue { get; }
        bool IfUnused { get; }
        bool IfEmpty { get; }
        bool Nowait { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "queue.delete-ok".</summary>
    internal interface IQueueDeleteOk : IMethod
    {
        uint MessageCount { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.qos".</summary>
    internal interface IBasicQos : IMethod
    {
        uint PrefetchSize { get; }
        ushort PrefetchCount { get; }
        bool Global { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.qos-ok".</summary>
    internal interface IBasicQosOk : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.consume".</summary>
    internal interface IBasicConsume : IMethod
    {
        ushort Reserved1 { get; }
        string Queue { get; }
        string ConsumerTag { get; }
        bool NoLocal { get; }
        bool NoAck { get; }
        bool Exclusive { get; }
        bool Nowait { get; }
        IDictionary<string, object> Arguments { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.consume-ok".</summary>
    internal interface IBasicConsumeOk : IMethod
    {
        string ConsumerTag { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.cancel".</summary>
    internal interface IBasicCancel : IMethod
    {
        string ConsumerTag { get; }
        bool Nowait { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.cancel-ok".</summary>
    internal interface IBasicCancelOk : IMethod
    {
        string ConsumerTag { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.publish".</summary>
    internal interface IBasicPublish : IMethod
    {
        ushort Reserved1 { get; }
        string Exchange { get; }
        string RoutingKey { get; }
        bool Mandatory { get; }
        bool Immediate { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.return".</summary>
    internal interface IBasicReturn : IMethod
    {
        ushort ReplyCode { get; }
        string ReplyText { get; }
        string Exchange { get; }
        string RoutingKey { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.deliver".</summary>
    internal interface IBasicDeliver : IMethod
    {
        string ConsumerTag { get; }
        ulong DeliveryTag { get; }
        bool Redelivered { get; }
        string Exchange { get; }
        string RoutingKey { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.get".</summary>
    internal interface IBasicGet : IMethod
    {
        ushort Reserved1 { get; }
        string Queue { get; }
        bool NoAck { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.get-ok".</summary>
    internal interface IBasicGetOk : IMethod
    {
        ulong DeliveryTag { get; }
        bool Redelivered { get; }
        string Exchange { get; }
        string RoutingKey { get; }
        uint MessageCount { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.get-empty".</summary>
    internal interface IBasicGetEmpty : IMethod
    {
        string Reserved1 { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.ack".</summary>
    internal interface IBasicAck : IMethod
    {
        ulong DeliveryTag { get; }
        bool Multiple { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.reject".</summary>
    internal interface IBasicReject : IMethod
    {
        ulong DeliveryTag { get; }
        bool Requeue { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.recover-async".</summary>
    internal interface IBasicRecoverAsync : IMethod
    {
        bool Requeue { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.recover".</summary>
    internal interface IBasicRecover : IMethod
    {
        bool Requeue { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.recover-ok".</summary>
    internal interface IBasicRecoverOk : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "basic.nack".</summary>
    internal interface IBasicNack : IMethod
    {
        ulong DeliveryTag { get; }
        bool Multiple { get; }
        bool Requeue { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "tx.select".</summary>
    internal interface ITxSelect : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "tx.select-ok".</summary>
    internal interface ITxSelectOk : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "tx.commit".</summary>
    internal interface ITxCommit : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "tx.commit-ok".</summary>
    internal interface ITxCommitOk : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "tx.rollback".</summary>
    internal interface ITxRollback : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "tx.rollback-ok".</summary>
    internal interface ITxRollbackOk : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification method "confirm.select".</summary>
    internal interface IConfirmSelect : IMethod
    {
        bool Nowait { get; }
    }
    /// <summary>Autogenerated type. AMQP specification method "confirm.select-ok".</summary>
    internal interface IConfirmSelectOk : IMethod
    {
    }
    /// <summary>Autogenerated type. AMQP specification content header properties for content class "basic"</summary>
    internal sealed class BasicProperties : RabbitMQ.Client.Impl.BasicProperties
    {
        private string _contentType;
        private string _contentEncoding;
        private IDictionary<string, object> _headers;
        private byte _deliveryMode;
        private byte _priority;
        private string _correlationId;
        private string _replyTo;
        private string _expiration;
        private string _messageId;
        private AmqpTimestamp _timestamp;
        private string _type;
        private string _userId;
        private string _appId;
        private string _clusterId;

        private bool _contentType_present = false;
        private bool _contentEncoding_present = false;
        private bool _headers_present = false;
        private bool _deliveryMode_present = false;
        private bool _priority_present = false;
        private bool _correlationId_present = false;
        private bool _replyTo_present = false;
        private bool _expiration_present = false;
        private bool _messageId_present = false;
        private bool _timestamp_present = false;
        private bool _type_present = false;
        private bool _userId_present = false;
        private bool _appId_present = false;
        private bool _clusterId_present = false;

        public override string ContentType
        {
            get => _contentType;
            set
            {
                _contentType_present = value != null;
                _contentType = value;
            }
        }

        public override string ContentEncoding
        {
            get => _contentEncoding;
            set
            {
                _contentEncoding_present = value != null;
                _contentEncoding = value;
            }
        }

        public override IDictionary<string, object> Headers
        {
            get => _headers;
            set
            {
                _headers_present = value != null;
                _headers = value;
            }
        }

        public override byte DeliveryMode
        {
            get => _deliveryMode;
            set
            {
                _deliveryMode_present = true;
                _deliveryMode = value;
            }
        }

        public override byte Priority
        {
            get => _priority;
            set
            {
                _priority_present = true;
                _priority = value;
            }
        }

        public override string CorrelationId
        {
            get => _correlationId;
            set
            {
                _correlationId_present = value != null;
                _correlationId = value;
            }
        }

        public override string ReplyTo
        {
            get => _replyTo;
            set
            {
                _replyTo_present = value != null;
                _replyTo = value;
            }
        }

        public override string Expiration
        {
            get => _expiration;
            set
            {
                _expiration_present = value != null;
                _expiration = value;
            }
        }

        public override string MessageId
        {
            get => _messageId;
            set
            {
                _messageId_present = value != null;
                _messageId = value;
            }
        }

        public override AmqpTimestamp Timestamp
        {
            get => _timestamp;
            set
            {
                _timestamp_present = true;
                _timestamp = value;
            }
        }

        public override string Type
        {
            get => _type;
            set
            {
                _type_present = value != null;
                _type = value;
            }
        }

        public override string UserId
        {
            get => _userId;
            set
            {
                _userId_present = value != null;
                _userId = value;
            }
        }

        public override string AppId
        {
            get => _appId;
            set
            {
                _appId_present = value != null;
                _appId = value;
            }
        }

        public override string ClusterId
        {
            get => _clusterId;
            set
            {
                _clusterId_present = value != null;
                _clusterId = value;
            }
        }

        public override void ClearContentType() => _contentType_present = false;

        public override void ClearContentEncoding() => _contentEncoding_present = false;

        public override void ClearHeaders() => _headers_present = false;

        public override void ClearDeliveryMode() => _deliveryMode_present = false;

        public override void ClearPriority() => _priority_present = false;

        public override void ClearCorrelationId() => _correlationId_present = false;

        public override void ClearReplyTo() => _replyTo_present = false;

        public override void ClearExpiration() => _expiration_present = false;

        public override void ClearMessageId() => _messageId_present = false;

        public override void ClearTimestamp() => _timestamp_present = false;

        public override void ClearType() => _type_present = false;

        public override void ClearUserId() => _userId_present = false;

        public override void ClearAppId() => _appId_present = false;

        public override void ClearClusterId() => _clusterId_present = false;

        public override bool IsContentTypePresent() => _contentType_present;

        public override bool IsContentEncodingPresent() => _contentEncoding_present;

        public override bool IsHeadersPresent() => _headers_present;

        public override bool IsDeliveryModePresent() => _deliveryMode_present;

        public override bool IsPriorityPresent() => _priority_present;

        public override bool IsCorrelationIdPresent() => _correlationId_present;

        public override bool IsReplyToPresent() => _replyTo_present;

        public override bool IsExpirationPresent() => _expiration_present;

        public override bool IsMessageIdPresent() => _messageId_present;

        public override bool IsTimestampPresent() => _timestamp_present;

        public override bool IsTypePresent() => _type_present;

        public override bool IsUserIdPresent() => _userId_present;

        public override bool IsAppIdPresent() => _appId_present;

        public override bool IsClusterIdPresent() => _clusterId_present;

        public BasicProperties() { }
        public override ushort ProtocolClassId => 60;
        public override string ProtocolClassName => "basic";

        internal override void ReadPropertiesFrom(ref Client.Impl.ContentHeaderPropertyReader reader)
        {
            _contentType_present = reader.ReadPresence();
            _contentEncoding_present = reader.ReadPresence();
            _headers_present = reader.ReadPresence();
            _deliveryMode_present = reader.ReadPresence();
            _priority_present = reader.ReadPresence();
            _correlationId_present = reader.ReadPresence();
            _replyTo_present = reader.ReadPresence();
            _expiration_present = reader.ReadPresence();
            _messageId_present = reader.ReadPresence();
            _timestamp_present = reader.ReadPresence();
            _type_present = reader.ReadPresence();
            _userId_present = reader.ReadPresence();
            _appId_present = reader.ReadPresence();
            _clusterId_present = reader.ReadPresence();
            reader.FinishPresence();
            if (_contentType_present) { _contentType = reader.ReadShortstr(); }
            if (_contentEncoding_present) { _contentEncoding = reader.ReadShortstr(); }
            if (_headers_present) { _headers = reader.ReadTable(); }
            if (_deliveryMode_present) { _deliveryMode = reader.ReadOctet(); }
            if (_priority_present) { _priority = reader.ReadOctet(); }
            if (_correlationId_present) { _correlationId = reader.ReadShortstr(); }
            if (_replyTo_present) { _replyTo = reader.ReadShortstr(); }
            if (_expiration_present) { _expiration = reader.ReadShortstr(); }
            if (_messageId_present) { _messageId = reader.ReadShortstr(); }
            if (_timestamp_present) { _timestamp = reader.ReadTimestamp(); }
            if (_type_present) { _type = reader.ReadShortstr(); }
            if (_userId_present) { _userId = reader.ReadShortstr(); }
            if (_appId_present) { _appId = reader.ReadShortstr(); }
            if (_clusterId_present) { _clusterId = reader.ReadShortstr(); }
        }

        internal override void WritePropertiesTo(ref Client.Impl.ContentHeaderPropertyWriter writer)
        {
            writer.WritePresence(_contentType_present);
            writer.WritePresence(_contentEncoding_present);
            writer.WritePresence(_headers_present);
            writer.WritePresence(_deliveryMode_present);
            writer.WritePresence(_priority_present);
            writer.WritePresence(_correlationId_present);
            writer.WritePresence(_replyTo_present);
            writer.WritePresence(_expiration_present);
            writer.WritePresence(_messageId_present);
            writer.WritePresence(_timestamp_present);
            writer.WritePresence(_type_present);
            writer.WritePresence(_userId_present);
            writer.WritePresence(_appId_present);
            writer.WritePresence(_clusterId_present);
            writer.FinishPresence();
            if (_contentType_present) { writer.WriteShortstr(_contentType); }
            if (_contentEncoding_present) { writer.WriteShortstr(_contentEncoding); }
            if (_headers_present) { writer.WriteTable(_headers); }
            if (_deliveryMode_present) { writer.WriteOctet(_deliveryMode); }
            if (_priority_present) { writer.WriteOctet(_priority); }
            if (_correlationId_present) { writer.WriteShortstr(_correlationId); }
            if (_replyTo_present) { writer.WriteShortstr(_replyTo); }
            if (_expiration_present) { writer.WriteShortstr(_expiration); }
            if (_messageId_present) { writer.WriteShortstr(_messageId); }
            if (_timestamp_present) { writer.WriteTimestamp(_timestamp); }
            if (_type_present) { writer.WriteShortstr(_type); }
            if (_userId_present) { writer.WriteShortstr(_userId); }
            if (_appId_present) { writer.WriteShortstr(_appId); }
            if (_clusterId_present) { writer.WriteShortstr(_clusterId); }
        }

        public override int GetRequiredPayloadBufferSize()
        {
            int bufferSize = 0;
            int fieldCount = 0;
            if (_contentType_present) { fieldCount++; bufferSize += 1 + Encoding.UTF8.GetByteCount(_contentType); } // _contentType in bytes
            if (_contentEncoding_present) { fieldCount++; bufferSize += 1 + Encoding.UTF8.GetByteCount(_contentEncoding); } // _contentEncoding in bytes
            if (_headers_present) { fieldCount++; bufferSize += WireFormatting.GetTableByteCount(_headers); } // _headers in bytes
            if (_deliveryMode_present) { fieldCount++; bufferSize++; } // _deliveryMode in bytes
            if (_priority_present) { fieldCount++; bufferSize++; } // _priority in bytes
            if (_correlationId_present) { fieldCount++; bufferSize += 1 + Encoding.UTF8.GetByteCount(_correlationId); } // _correlationId in bytes
            if (_replyTo_present) { fieldCount++; bufferSize += 1 + Encoding.UTF8.GetByteCount(_replyTo); } // _replyTo in bytes
            if (_expiration_present) { fieldCount++; bufferSize += 1 + Encoding.UTF8.GetByteCount(_expiration); } // _expiration in bytes
            if (_messageId_present) { fieldCount++; bufferSize += 1 + Encoding.UTF8.GetByteCount(_messageId); } // _messageId in bytes
            if (_timestamp_present) { fieldCount++; bufferSize += 8; } // _timestamp in bytes
            if (_type_present) { fieldCount++; bufferSize += 1 + Encoding.UTF8.GetByteCount(_type); } // _type in bytes
            if (_userId_present) { fieldCount++; bufferSize += 1 + Encoding.UTF8.GetByteCount(_userId); } // _userId in bytes
            if (_appId_present) { fieldCount++; bufferSize += 1 + Encoding.UTF8.GetByteCount(_appId); } // _appId in bytes
            if (_clusterId_present) { fieldCount++; bufferSize += 1 + Encoding.UTF8.GetByteCount(_clusterId); } // _clusterId in bytes
            bufferSize += Math.Max((int)Math.Ceiling(fieldCount / 15.0), 1) * 2; // number of presence fields in bytes
            return bufferSize;
        }

        public override void AppendPropertyDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append("content-type="); sb.Append(_contentType_present ? (_contentType == null ? "(null)" : _contentType.ToString()) : "_"); sb.Append(", ");
            sb.Append("content-encoding="); sb.Append(_contentEncoding_present ? (_contentEncoding == null ? "(null)" : _contentEncoding.ToString()) : "_"); sb.Append(", ");
            sb.Append("headers="); sb.Append(_headers_present ? (_headers == null ? "(null)" : _headers.ToString()) : "_"); sb.Append(", ");
            sb.Append("delivery-mode="); sb.Append(_deliveryMode_present ? _deliveryMode.ToString() : "_"); sb.Append(", ");
            sb.Append("priority="); sb.Append(_priority_present ? _priority.ToString() : "_"); sb.Append(", ");
            sb.Append("correlation-id="); sb.Append(_correlationId_present ? (_correlationId == null ? "(null)" : _correlationId.ToString()) : "_"); sb.Append(", ");
            sb.Append("reply-to="); sb.Append(_replyTo_present ? (_replyTo == null ? "(null)" : _replyTo.ToString()) : "_"); sb.Append(", ");
            sb.Append("expiration="); sb.Append(_expiration_present ? (_expiration == null ? "(null)" : _expiration.ToString()) : "_"); sb.Append(", ");
            sb.Append("message-id="); sb.Append(_messageId_present ? (_messageId == null ? "(null)" : _messageId.ToString()) : "_"); sb.Append(", ");
            sb.Append("timestamp="); sb.Append(_timestamp_present ? _timestamp.ToString() : "_"); sb.Append(", ");
            sb.Append("type="); sb.Append(_type_present ? (_type == null ? "(null)" : _type.ToString()) : "_"); sb.Append(", ");
            sb.Append("user-id="); sb.Append(_userId_present ? (_userId == null ? "(null)" : _userId.ToString()) : "_"); sb.Append(", ");
            sb.Append("app-id="); sb.Append(_appId_present ? (_appId == null ? "(null)" : _appId.ToString()) : "_"); sb.Append(", ");
            sb.Append("cluster-id="); sb.Append(_clusterId_present ? (_clusterId == null ? "(null)" : _clusterId.ToString()) : "_");
            sb.Append(")");
        }
    }
}
namespace RabbitMQ.Client.Framing.Impl
{
    internal static class ClassConstants
    {
        internal const ushort Connection = 10;
        internal const ushort Channel = 20;
        internal const ushort Exchange = 40;
        internal const ushort Queue = 50;
        internal const ushort Basic = 60;
        internal const ushort Tx = 90;
        internal const ushort Confirm = 85;
    }

    internal enum ClassId
    {
        Connection = 10,
        Channel = 20,
        Exchange = 40,
        Queue = 50,
        Basic = 60,
        Tx = 90,
        Confirm = 85,
        Invalid = -1,
    }

    internal static class ConnectionMethodConstants
    {
        internal const ushort Start = 10;
        internal const ushort StartOk = 11;
        internal const ushort Secure = 20;
        internal const ushort SecureOk = 21;
        internal const ushort Tune = 30;
        internal const ushort TuneOk = 31;
        internal const ushort Open = 40;
        internal const ushort OpenOk = 41;
        internal const ushort Close = 50;
        internal const ushort CloseOk = 51;
        internal const ushort Blocked = 60;
        internal const ushort Unblocked = 61;
        internal const ushort UpdateSecret = 70;
        internal const ushort UpdateSecretOk = 71;
    }
    internal static class ChannelMethodConstants
    {
        internal const ushort Open = 10;
        internal const ushort OpenOk = 11;
        internal const ushort Flow = 20;
        internal const ushort FlowOk = 21;
        internal const ushort Close = 40;
        internal const ushort CloseOk = 41;
    }
    internal static class ExchangeMethodConstants
    {
        internal const ushort Declare = 10;
        internal const ushort DeclareOk = 11;
        internal const ushort Delete = 20;
        internal const ushort DeleteOk = 21;
        internal const ushort Bind = 30;
        internal const ushort BindOk = 31;
        internal const ushort Unbind = 40;
        internal const ushort UnbindOk = 51;
    }
    internal static class QueueMethodConstants
    {
        internal const ushort Declare = 10;
        internal const ushort DeclareOk = 11;
        internal const ushort Bind = 20;
        internal const ushort BindOk = 21;
        internal const ushort Unbind = 50;
        internal const ushort UnbindOk = 51;
        internal const ushort Purge = 30;
        internal const ushort PurgeOk = 31;
        internal const ushort Delete = 40;
        internal const ushort DeleteOk = 41;
    }
    internal static class BasicMethodConstants
    {
        internal const ushort Qos = 10;
        internal const ushort QosOk = 11;
        internal const ushort Consume = 20;
        internal const ushort ConsumeOk = 21;
        internal const ushort Cancel = 30;
        internal const ushort CancelOk = 31;
        internal const ushort Publish = 40;
        internal const ushort Return = 50;
        internal const ushort Deliver = 60;
        internal const ushort Get = 70;
        internal const ushort GetOk = 71;
        internal const ushort GetEmpty = 72;
        internal const ushort Ack = 80;
        internal const ushort Reject = 90;
        internal const ushort RecoverAsync = 100;
        internal const ushort Recover = 110;
        internal const ushort RecoverOk = 111;
        internal const ushort Nack = 120;
    }
    internal static class TxMethodConstants
    {
        internal const ushort Select = 10;
        internal const ushort SelectOk = 11;
        internal const ushort Commit = 20;
        internal const ushort CommitOk = 21;
        internal const ushort Rollback = 30;
        internal const ushort RollbackOk = 31;
    }
    internal static class ConfirmMethodConstants
    {
        internal const ushort Select = 10;
        internal const ushort SelectOk = 11;
    }

    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConnectionStart : Client.Impl.MethodBase, IConnectionStart
    {

        public byte _versionMajor;
        public byte _versionMinor;
        public IDictionary<string, object> _serverProperties;
        public byte[] _mechanisms;
        public byte[] _locales;

        byte IConnectionStart.VersionMajor => _versionMajor;
        byte IConnectionStart.VersionMinor => _versionMinor;
        IDictionary<string, object> IConnectionStart.ServerProperties => _serverProperties;
        byte[] IConnectionStart.Mechanisms => _mechanisms;
        byte[] IConnectionStart.Locales => _locales;

        public ConnectionStart() { }
        public ConnectionStart(byte @VersionMajor, byte @VersionMinor, IDictionary<string, object> @ServerProperties, byte[] @Mechanisms, byte[] @Locales)
        {
            _versionMajor = @VersionMajor;
            _versionMinor = @VersionMinor;
            _serverProperties = @ServerProperties;
            _mechanisms = @Mechanisms;
            _locales = @Locales;
        }

        public override ushort ProtocolClassId => ClassConstants.Connection;
        public override ushort ProtocolMethodId => ConnectionMethodConstants.Start;
        public override string ProtocolMethodName => "connection.start";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _versionMajor = reader.ReadOctet();
            _versionMinor = reader.ReadOctet();
            _serverProperties = reader.ReadTable();
            _mechanisms = reader.ReadLongstr();
            _locales = reader.ReadLongstr();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteOctet(_versionMajor);
            writer.WriteOctet(_versionMinor);
            writer.WriteTable(_serverProperties);
            writer.WriteLongstr(_mechanisms);
            writer.WriteLongstr(_locales);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize++; // _versionMajor in bytes
            bufferSize++; // _versionMinor in bytes
            bufferSize += WireFormatting.GetTableByteCount(_serverProperties); // _serverProperties in bytes
            bufferSize += 4 + _mechanisms.Length; // _mechanisms in bytes
            bufferSize += 4 + _locales.Length; // _locales in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_versionMajor); sb.Append(",");
            sb.Append(_versionMinor); sb.Append(",");
            sb.Append(_serverProperties); sb.Append(",");
            sb.Append(_mechanisms); sb.Append(",");
            sb.Append(_locales);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConnectionStartOk : Client.Impl.MethodBase, IConnectionStartOk
    {

        public IDictionary<string, object> _clientProperties;
        public string _mechanism;
        public byte[] _response;
        public string _locale;

        IDictionary<string, object> IConnectionStartOk.ClientProperties => _clientProperties;
        string IConnectionStartOk.Mechanism => _mechanism;
        byte[] IConnectionStartOk.Response => _response;
        string IConnectionStartOk.Locale => _locale;

        public ConnectionStartOk() { }
        public ConnectionStartOk(IDictionary<string, object> @ClientProperties, string @Mechanism, byte[] @Response, string @Locale)
        {
            _clientProperties = @ClientProperties;
            _mechanism = @Mechanism;
            _response = @Response;
            _locale = @Locale;
        }

        public override ushort ProtocolClassId => ClassConstants.Connection;
        public override ushort ProtocolMethodId => ConnectionMethodConstants.StartOk;
        public override string ProtocolMethodName => "connection.start-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _clientProperties = reader.ReadTable();
            _mechanism = reader.ReadShortstr();
            _response = reader.ReadLongstr();
            _locale = reader.ReadShortstr();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteTable(_clientProperties);
            writer.WriteShortstr(_mechanism);
            writer.WriteLongstr(_response);
            writer.WriteShortstr(_locale);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += WireFormatting.GetTableByteCount(_clientProperties); // _clientProperties in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_mechanism); // _mechanism in bytes
            bufferSize += 4 + _response.Length; // _response in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_locale); // _locale in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_clientProperties); sb.Append(",");
            sb.Append(_mechanism); sb.Append(",");
            sb.Append(_response); sb.Append(",");
            sb.Append(_locale);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConnectionSecure : Client.Impl.MethodBase, IConnectionSecure
    {

        public byte[] _challenge;

        byte[] IConnectionSecure.Challenge => _challenge;

        public ConnectionSecure() { }
        public ConnectionSecure(byte[] @Challenge)
        {
            _challenge = @Challenge;
        }

        public override ushort ProtocolClassId => ClassConstants.Connection;
        public override ushort ProtocolMethodId => ConnectionMethodConstants.Secure;
        public override string ProtocolMethodName => "connection.secure";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _challenge = reader.ReadLongstr();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteLongstr(_challenge);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 4 + _challenge.Length; // _challenge in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_challenge);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConnectionSecureOk : Client.Impl.MethodBase, IConnectionSecureOk
    {

        public byte[] _response;

        byte[] IConnectionSecureOk.Response => _response;

        public ConnectionSecureOk() { }
        public ConnectionSecureOk(byte[] @Response)
        {
            _response = @Response;
        }

        public override ushort ProtocolClassId => ClassConstants.Connection;
        public override ushort ProtocolMethodId => ConnectionMethodConstants.SecureOk;
        public override string ProtocolMethodName => "connection.secure-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _response = reader.ReadLongstr();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteLongstr(_response);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 4 + _response.Length; // _response in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_response);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConnectionTune : Client.Impl.MethodBase, IConnectionTune
    {

        public ushort _channelMax;
        public uint _frameMax;
        public ushort _heartbeat;

        ushort IConnectionTune.ChannelMax => _channelMax;
        uint IConnectionTune.FrameMax => _frameMax;
        ushort IConnectionTune.Heartbeat => _heartbeat;

        public ConnectionTune() { }
        public ConnectionTune(ushort @ChannelMax, uint @FrameMax, ushort @Heartbeat)
        {
            _channelMax = @ChannelMax;
            _frameMax = @FrameMax;
            _heartbeat = @Heartbeat;
        }

        public override ushort ProtocolClassId => ClassConstants.Connection;
        public override ushort ProtocolMethodId => ConnectionMethodConstants.Tune;
        public override string ProtocolMethodName => "connection.tune";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _channelMax = reader.ReadShort();
            _frameMax = reader.ReadLong();
            _heartbeat = reader.ReadShort();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_channelMax);
            writer.WriteLong(_frameMax);
            writer.WriteShort(_heartbeat);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _channelMax in bytes
            bufferSize += 4; // _frameMax in bytes
            bufferSize += 2; // _heartbeat in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_channelMax); sb.Append(",");
            sb.Append(_frameMax); sb.Append(",");
            sb.Append(_heartbeat);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConnectionTuneOk : Client.Impl.MethodBase, IConnectionTuneOk
    {

        public ushort _channelMax;
        public uint _frameMax;
        public ushort _heartbeat;

        ushort IConnectionTuneOk.ChannelMax => _channelMax;
        uint IConnectionTuneOk.FrameMax => _frameMax;
        ushort IConnectionTuneOk.Heartbeat => _heartbeat;

        public ConnectionTuneOk() { }
        public ConnectionTuneOk(ushort @ChannelMax, uint @FrameMax, ushort @Heartbeat)
        {
            _channelMax = @ChannelMax;
            _frameMax = @FrameMax;
            _heartbeat = @Heartbeat;
        }

        public override ushort ProtocolClassId => ClassConstants.Connection;
        public override ushort ProtocolMethodId => ConnectionMethodConstants.TuneOk;
        public override string ProtocolMethodName => "connection.tune-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _channelMax = reader.ReadShort();
            _frameMax = reader.ReadLong();
            _heartbeat = reader.ReadShort();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_channelMax);
            writer.WriteLong(_frameMax);
            writer.WriteShort(_heartbeat);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _channelMax in bytes
            bufferSize += 4; // _frameMax in bytes
            bufferSize += 2; // _heartbeat in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_channelMax); sb.Append(",");
            sb.Append(_frameMax); sb.Append(",");
            sb.Append(_heartbeat);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConnectionOpen : Client.Impl.MethodBase, IConnectionOpen
    {

        public string _virtualHost;
        public string _reserved1;
        public bool _reserved2;

        string IConnectionOpen.VirtualHost => _virtualHost;
        string IConnectionOpen.Reserved1 => _reserved1;
        bool IConnectionOpen.Reserved2 => _reserved2;

        public ConnectionOpen() { }
        public ConnectionOpen(string @VirtualHost, string @Reserved1, bool @Reserved2)
        {
            _virtualHost = @VirtualHost;
            _reserved1 = @Reserved1;
            _reserved2 = @Reserved2;
        }

        public override ushort ProtocolClassId => ClassConstants.Connection;
        public override ushort ProtocolMethodId => ConnectionMethodConstants.Open;
        public override string ProtocolMethodName => "connection.open";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _virtualHost = reader.ReadShortstr();
            _reserved1 = reader.ReadShortstr();
            _reserved2 = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShortstr(_virtualHost);
            writer.WriteShortstr(_reserved1);
            writer.WriteBit(_reserved2);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_virtualHost); // _virtualHost in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_reserved1); // _reserved1 in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_virtualHost); sb.Append(",");
            sb.Append(_reserved1); sb.Append(",");
            sb.Append(_reserved2);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConnectionOpenOk : Client.Impl.MethodBase, IConnectionOpenOk
    {

        public string _reserved1;

        string IConnectionOpenOk.Reserved1 => _reserved1;

        public ConnectionOpenOk() { }
        public ConnectionOpenOk(string @Reserved1)
        {
            _reserved1 = @Reserved1;
        }

        public override ushort ProtocolClassId => ClassConstants.Connection;
        public override ushort ProtocolMethodId => ConnectionMethodConstants.OpenOk;
        public override string ProtocolMethodName => "connection.open-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadShortstr();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShortstr(_reserved1);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_reserved1); // _reserved1 in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConnectionClose : Client.Impl.MethodBase, IConnectionClose
    {

        public ushort _replyCode;
        public string _replyText;
        public ushort _classId;
        public ushort _methodId;

        ushort IConnectionClose.ReplyCode => _replyCode;
        string IConnectionClose.ReplyText => _replyText;
        ushort IConnectionClose.ClassId => _classId;
        ushort IConnectionClose.MethodId => _methodId;

        public ConnectionClose() { }
        public ConnectionClose(ushort @ReplyCode, string @ReplyText, ushort @ClassId, ushort @MethodId)
        {
            _replyCode = @ReplyCode;
            _replyText = @ReplyText;
            _classId = @ClassId;
            _methodId = @MethodId;
        }

        public override ushort ProtocolClassId => ClassConstants.Connection;
        public override ushort ProtocolMethodId => ConnectionMethodConstants.Close;
        public override string ProtocolMethodName => "connection.close";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _replyCode = reader.ReadShort();
            _replyText = reader.ReadShortstr();
            _classId = reader.ReadShort();
            _methodId = reader.ReadShort();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_replyCode);
            writer.WriteShortstr(_replyText);
            writer.WriteShort(_classId);
            writer.WriteShort(_methodId);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _replyCode in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_replyText); // _replyText in bytes
            bufferSize += 2; // _classId in bytes
            bufferSize += 2; // _methodId in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_replyCode); sb.Append(",");
            sb.Append(_replyText); sb.Append(",");
            sb.Append(_classId); sb.Append(",");
            sb.Append(_methodId);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConnectionCloseOk : Client.Impl.MethodBase, IConnectionCloseOk
    {



        public ConnectionCloseOk()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Connection;
        public override ushort ProtocolMethodId => ConnectionMethodConstants.CloseOk;
        public override string ProtocolMethodName => "connection.close-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConnectionBlocked : Client.Impl.MethodBase, IConnectionBlocked
    {

        public string _reason;

        string IConnectionBlocked.Reason => _reason;

        public ConnectionBlocked() { }
        public ConnectionBlocked(string @Reason)
        {
            _reason = @Reason;
        }

        public override ushort ProtocolClassId => ClassConstants.Connection;
        public override ushort ProtocolMethodId => ConnectionMethodConstants.Blocked;
        public override string ProtocolMethodName => "connection.blocked";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reason = reader.ReadShortstr();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShortstr(_reason);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_reason); // _reason in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reason);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConnectionUnblocked : Client.Impl.MethodBase, IConnectionUnblocked
    {



        public ConnectionUnblocked()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Connection;
        public override ushort ProtocolMethodId => ConnectionMethodConstants.Unblocked;
        public override string ProtocolMethodName => "connection.unblocked";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConnectionUpdateSecret : Client.Impl.MethodBase, IConnectionUpdateSecret
    {

        public byte[] _newSecret;
        public string _reason;

        byte[] IConnectionUpdateSecret.NewSecret => _newSecret;
        string IConnectionUpdateSecret.Reason => _reason;

        public ConnectionUpdateSecret() { }
        public ConnectionUpdateSecret(byte[] @NewSecret, string @Reason)
        {
            _newSecret = @NewSecret;
            _reason = @Reason;
        }

        public override ushort ProtocolClassId => ClassConstants.Connection;
        public override ushort ProtocolMethodId => ConnectionMethodConstants.UpdateSecret;
        public override string ProtocolMethodName => "connection.update-secret";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _newSecret = reader.ReadLongstr();
            _reason = reader.ReadShortstr();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteLongstr(_newSecret);
            writer.WriteShortstr(_reason);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 4 + _newSecret.Length; // _newSecret in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_reason); // _reason in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_newSecret); sb.Append(",");
            sb.Append(_reason);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConnectionUpdateSecretOk : Client.Impl.MethodBase, IConnectionUpdateSecretOk
    {



        public ConnectionUpdateSecretOk()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Connection;
        public override ushort ProtocolMethodId => ConnectionMethodConstants.UpdateSecretOk;
        public override string ProtocolMethodName => "connection.update-secret-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ChannelOpen : Client.Impl.MethodBase, IChannelOpen
    {

        public string _reserved1;

        string IChannelOpen.Reserved1 => _reserved1;

        public ChannelOpen() { }
        public ChannelOpen(string @Reserved1)
        {
            _reserved1 = @Reserved1;
        }

        public override ushort ProtocolClassId => ClassConstants.Channel;
        public override ushort ProtocolMethodId => ChannelMethodConstants.Open;
        public override string ProtocolMethodName => "channel.open";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadShortstr();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShortstr(_reserved1);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_reserved1); // _reserved1 in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ChannelOpenOk : Client.Impl.MethodBase, IChannelOpenOk
    {

        public byte[] _reserved1;

        byte[] IChannelOpenOk.Reserved1 => _reserved1;

        public ChannelOpenOk() { }
        public ChannelOpenOk(byte[] @Reserved1)
        {
            _reserved1 = @Reserved1;
        }

        public override ushort ProtocolClassId => ClassConstants.Channel;
        public override ushort ProtocolMethodId => ChannelMethodConstants.OpenOk;
        public override string ProtocolMethodName => "channel.open-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadLongstr();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteLongstr(_reserved1);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 4 + _reserved1.Length; // _reserved1 in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ChannelFlow : Client.Impl.MethodBase, IChannelFlow
    {

        public bool _active;

        bool IChannelFlow.Active => _active;

        public ChannelFlow() { }
        public ChannelFlow(bool @Active)
        {
            _active = @Active;
        }

        public override ushort ProtocolClassId => ClassConstants.Channel;
        public override ushort ProtocolMethodId => ChannelMethodConstants.Flow;
        public override string ProtocolMethodName => "channel.flow";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _active = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteBit(_active);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_active);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ChannelFlowOk : Client.Impl.MethodBase, IChannelFlowOk
    {

        public bool _active;

        bool IChannelFlowOk.Active => _active;

        public ChannelFlowOk() { }
        public ChannelFlowOk(bool @Active)
        {
            _active = @Active;
        }

        public override ushort ProtocolClassId => ClassConstants.Channel;
        public override ushort ProtocolMethodId => ChannelMethodConstants.FlowOk;
        public override string ProtocolMethodName => "channel.flow-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _active = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteBit(_active);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_active);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ChannelClose : Client.Impl.MethodBase, IChannelClose
    {

        public ushort _replyCode;
        public string _replyText;
        public ushort _classId;
        public ushort _methodId;

        ushort IChannelClose.ReplyCode => _replyCode;
        string IChannelClose.ReplyText => _replyText;
        ushort IChannelClose.ClassId => _classId;
        ushort IChannelClose.MethodId => _methodId;

        public ChannelClose() { }
        public ChannelClose(ushort @ReplyCode, string @ReplyText, ushort @ClassId, ushort @MethodId)
        {
            _replyCode = @ReplyCode;
            _replyText = @ReplyText;
            _classId = @ClassId;
            _methodId = @MethodId;
        }

        public override ushort ProtocolClassId => ClassConstants.Channel;
        public override ushort ProtocolMethodId => ChannelMethodConstants.Close;
        public override string ProtocolMethodName => "channel.close";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _replyCode = reader.ReadShort();
            _replyText = reader.ReadShortstr();
            _classId = reader.ReadShort();
            _methodId = reader.ReadShort();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_replyCode);
            writer.WriteShortstr(_replyText);
            writer.WriteShort(_classId);
            writer.WriteShort(_methodId);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _replyCode in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_replyText); // _replyText in bytes
            bufferSize += 2; // _classId in bytes
            bufferSize += 2; // _methodId in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_replyCode); sb.Append(",");
            sb.Append(_replyText); sb.Append(",");
            sb.Append(_classId); sb.Append(",");
            sb.Append(_methodId);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ChannelCloseOk : Client.Impl.MethodBase, IChannelCloseOk
    {



        public ChannelCloseOk()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Channel;
        public override ushort ProtocolMethodId => ChannelMethodConstants.CloseOk;
        public override string ProtocolMethodName => "channel.close-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ExchangeDeclare : Client.Impl.MethodBase, IExchangeDeclare
    {

        public ushort _reserved1;
        public string _exchange;
        public string _type;
        public bool _passive;
        public bool _durable;
        public bool _autoDelete;
        public bool _internal;
        public bool _nowait;
        public IDictionary<string, object> _arguments;

        ushort IExchangeDeclare.Reserved1 => _reserved1;
        string IExchangeDeclare.Exchange => _exchange;
        string IExchangeDeclare.Type => _type;
        bool IExchangeDeclare.Passive => _passive;
        bool IExchangeDeclare.Durable => _durable;
        bool IExchangeDeclare.AutoDelete => _autoDelete;
        bool IExchangeDeclare.Internal => _internal;
        bool IExchangeDeclare.Nowait => _nowait;
        IDictionary<string, object> IExchangeDeclare.Arguments => _arguments;

        public ExchangeDeclare() { }
        public ExchangeDeclare(ushort @Reserved1, string @Exchange, string @Type, bool @Passive, bool @Durable, bool @AutoDelete, bool @Internal, bool @Nowait, IDictionary<string, object> @Arguments)
        {
            _reserved1 = @Reserved1;
            _exchange = @Exchange;
            _type = @Type;
            _passive = @Passive;
            _durable = @Durable;
            _autoDelete = @AutoDelete;
            _internal = @Internal;
            _nowait = @Nowait;
            _arguments = @Arguments;
        }

        public override ushort ProtocolClassId => ClassConstants.Exchange;
        public override ushort ProtocolMethodId => ExchangeMethodConstants.Declare;
        public override string ProtocolMethodName => "exchange.declare";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadShort();
            _exchange = reader.ReadShortstr();
            _type = reader.ReadShortstr();
            _passive = reader.ReadBit();
            _durable = reader.ReadBit();
            _autoDelete = reader.ReadBit();
            _internal = reader.ReadBit();
            _nowait = reader.ReadBit();
            _arguments = reader.ReadTable();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_reserved1);
            writer.WriteShortstr(_exchange);
            writer.WriteShortstr(_type);
            writer.WriteBit(_passive);
            writer.WriteBit(_durable);
            writer.WriteBit(_autoDelete);
            writer.WriteBit(_internal);
            writer.WriteBit(_nowait);
            writer.WriteTable(_arguments);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _reserved1 in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_exchange); // _exchange in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_type); // _type in bytes
            bufferSize += WireFormatting.GetTableByteCount(_arguments); // _arguments in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1); sb.Append(",");
            sb.Append(_exchange); sb.Append(",");
            sb.Append(_type); sb.Append(",");
            sb.Append(_passive); sb.Append(",");
            sb.Append(_durable); sb.Append(",");
            sb.Append(_autoDelete); sb.Append(",");
            sb.Append(_internal); sb.Append(",");
            sb.Append(_nowait); sb.Append(",");
            sb.Append(_arguments);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ExchangeDeclareOk : Client.Impl.MethodBase, IExchangeDeclareOk
    {



        public ExchangeDeclareOk()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Exchange;
        public override ushort ProtocolMethodId => ExchangeMethodConstants.DeclareOk;
        public override string ProtocolMethodName => "exchange.declare-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ExchangeDelete : Client.Impl.MethodBase, IExchangeDelete
    {

        public ushort _reserved1;
        public string _exchange;
        public bool _ifUnused;
        public bool _nowait;

        ushort IExchangeDelete.Reserved1 => _reserved1;
        string IExchangeDelete.Exchange => _exchange;
        bool IExchangeDelete.IfUnused => _ifUnused;
        bool IExchangeDelete.Nowait => _nowait;

        public ExchangeDelete() { }
        public ExchangeDelete(ushort @Reserved1, string @Exchange, bool @IfUnused, bool @Nowait)
        {
            _reserved1 = @Reserved1;
            _exchange = @Exchange;
            _ifUnused = @IfUnused;
            _nowait = @Nowait;
        }

        public override ushort ProtocolClassId => ClassConstants.Exchange;
        public override ushort ProtocolMethodId => ExchangeMethodConstants.Delete;
        public override string ProtocolMethodName => "exchange.delete";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadShort();
            _exchange = reader.ReadShortstr();
            _ifUnused = reader.ReadBit();
            _nowait = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_reserved1);
            writer.WriteShortstr(_exchange);
            writer.WriteBit(_ifUnused);
            writer.WriteBit(_nowait);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _reserved1 in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_exchange); // _exchange in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1); sb.Append(",");
            sb.Append(_exchange); sb.Append(",");
            sb.Append(_ifUnused); sb.Append(",");
            sb.Append(_nowait);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ExchangeDeleteOk : Client.Impl.MethodBase, IExchangeDeleteOk
    {



        public ExchangeDeleteOk()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Exchange;
        public override ushort ProtocolMethodId => ExchangeMethodConstants.DeleteOk;
        public override string ProtocolMethodName => "exchange.delete-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ExchangeBind : Client.Impl.MethodBase, IExchangeBind
    {

        public ushort _reserved1;
        public string _destination;
        public string _source;
        public string _routingKey;
        public bool _nowait;
        public IDictionary<string, object> _arguments;

        ushort IExchangeBind.Reserved1 => _reserved1;
        string IExchangeBind.Destination => _destination;
        string IExchangeBind.Source => _source;
        string IExchangeBind.RoutingKey => _routingKey;
        bool IExchangeBind.Nowait => _nowait;
        IDictionary<string, object> IExchangeBind.Arguments => _arguments;

        public ExchangeBind() { }
        public ExchangeBind(ushort @Reserved1, string @Destination, string @Source, string @RoutingKey, bool @Nowait, IDictionary<string, object> @Arguments)
        {
            _reserved1 = @Reserved1;
            _destination = @Destination;
            _source = @Source;
            _routingKey = @RoutingKey;
            _nowait = @Nowait;
            _arguments = @Arguments;
        }

        public override ushort ProtocolClassId => ClassConstants.Exchange;
        public override ushort ProtocolMethodId => ExchangeMethodConstants.Bind;
        public override string ProtocolMethodName => "exchange.bind";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadShort();
            _destination = reader.ReadShortstr();
            _source = reader.ReadShortstr();
            _routingKey = reader.ReadShortstr();
            _nowait = reader.ReadBit();
            _arguments = reader.ReadTable();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_reserved1);
            writer.WriteShortstr(_destination);
            writer.WriteShortstr(_source);
            writer.WriteShortstr(_routingKey);
            writer.WriteBit(_nowait);
            writer.WriteTable(_arguments);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _reserved1 in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_destination); // _destination in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_source); // _source in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_routingKey); // _routingKey in bytes
            bufferSize += WireFormatting.GetTableByteCount(_arguments); // _arguments in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1); sb.Append(",");
            sb.Append(_destination); sb.Append(",");
            sb.Append(_source); sb.Append(",");
            sb.Append(_routingKey); sb.Append(",");
            sb.Append(_nowait); sb.Append(",");
            sb.Append(_arguments);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ExchangeBindOk : Client.Impl.MethodBase, IExchangeBindOk
    {



        public ExchangeBindOk()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Exchange;
        public override ushort ProtocolMethodId => ExchangeMethodConstants.BindOk;
        public override string ProtocolMethodName => "exchange.bind-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ExchangeUnbind : Client.Impl.MethodBase, IExchangeUnbind
    {

        public ushort _reserved1;
        public string _destination;
        public string _source;
        public string _routingKey;
        public bool _nowait;
        public IDictionary<string, object> _arguments;

        ushort IExchangeUnbind.Reserved1 => _reserved1;
        string IExchangeUnbind.Destination => _destination;
        string IExchangeUnbind.Source => _source;
        string IExchangeUnbind.RoutingKey => _routingKey;
        bool IExchangeUnbind.Nowait => _nowait;
        IDictionary<string, object> IExchangeUnbind.Arguments => _arguments;

        public ExchangeUnbind() { }
        public ExchangeUnbind(ushort @Reserved1, string @Destination, string @Source, string @RoutingKey, bool @Nowait, IDictionary<string, object> @Arguments)
        {
            _reserved1 = @Reserved1;
            _destination = @Destination;
            _source = @Source;
            _routingKey = @RoutingKey;
            _nowait = @Nowait;
            _arguments = @Arguments;
        }

        public override ushort ProtocolClassId => ClassConstants.Exchange;
        public override ushort ProtocolMethodId => ExchangeMethodConstants.Unbind;
        public override string ProtocolMethodName => "exchange.unbind";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadShort();
            _destination = reader.ReadShortstr();
            _source = reader.ReadShortstr();
            _routingKey = reader.ReadShortstr();
            _nowait = reader.ReadBit();
            _arguments = reader.ReadTable();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_reserved1);
            writer.WriteShortstr(_destination);
            writer.WriteShortstr(_source);
            writer.WriteShortstr(_routingKey);
            writer.WriteBit(_nowait);
            writer.WriteTable(_arguments);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _reserved1 in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_destination); // _destination in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_source); // _source in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_routingKey); // _routingKey in bytes
            bufferSize += WireFormatting.GetTableByteCount(_arguments); // _arguments in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1); sb.Append(",");
            sb.Append(_destination); sb.Append(",");
            sb.Append(_source); sb.Append(",");
            sb.Append(_routingKey); sb.Append(",");
            sb.Append(_nowait); sb.Append(",");
            sb.Append(_arguments);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ExchangeUnbindOk : Client.Impl.MethodBase, IExchangeUnbindOk
    {



        public ExchangeUnbindOk()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Exchange;
        public override ushort ProtocolMethodId => ExchangeMethodConstants.UnbindOk;
        public override string ProtocolMethodName => "exchange.unbind-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class QueueDeclare : Client.Impl.MethodBase, IQueueDeclare
    {

        public ushort _reserved1;
        public string _queue;
        public bool _passive;
        public bool _durable;
        public bool _exclusive;
        public bool _autoDelete;
        public bool _nowait;
        public IDictionary<string, object> _arguments;

        ushort IQueueDeclare.Reserved1 => _reserved1;
        string IQueueDeclare.Queue => _queue;
        bool IQueueDeclare.Passive => _passive;
        bool IQueueDeclare.Durable => _durable;
        bool IQueueDeclare.Exclusive => _exclusive;
        bool IQueueDeclare.AutoDelete => _autoDelete;
        bool IQueueDeclare.Nowait => _nowait;
        IDictionary<string, object> IQueueDeclare.Arguments => _arguments;

        public QueueDeclare() { }
        public QueueDeclare(ushort @Reserved1, string @Queue, bool @Passive, bool @Durable, bool @Exclusive, bool @AutoDelete, bool @Nowait, IDictionary<string, object> @Arguments)
        {
            _reserved1 = @Reserved1;
            _queue = @Queue;
            _passive = @Passive;
            _durable = @Durable;
            _exclusive = @Exclusive;
            _autoDelete = @AutoDelete;
            _nowait = @Nowait;
            _arguments = @Arguments;
        }

        public override ushort ProtocolClassId => ClassConstants.Queue;
        public override ushort ProtocolMethodId => QueueMethodConstants.Declare;
        public override string ProtocolMethodName => "queue.declare";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadShort();
            _queue = reader.ReadShortstr();
            _passive = reader.ReadBit();
            _durable = reader.ReadBit();
            _exclusive = reader.ReadBit();
            _autoDelete = reader.ReadBit();
            _nowait = reader.ReadBit();
            _arguments = reader.ReadTable();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_reserved1);
            writer.WriteShortstr(_queue);
            writer.WriteBit(_passive);
            writer.WriteBit(_durable);
            writer.WriteBit(_exclusive);
            writer.WriteBit(_autoDelete);
            writer.WriteBit(_nowait);
            writer.WriteTable(_arguments);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _reserved1 in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_queue); // _queue in bytes
            bufferSize += WireFormatting.GetTableByteCount(_arguments); // _arguments in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1); sb.Append(",");
            sb.Append(_queue); sb.Append(",");
            sb.Append(_passive); sb.Append(",");
            sb.Append(_durable); sb.Append(",");
            sb.Append(_exclusive); sb.Append(",");
            sb.Append(_autoDelete); sb.Append(",");
            sb.Append(_nowait); sb.Append(",");
            sb.Append(_arguments);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class QueueDeclareOk : Client.Impl.MethodBase, IQueueDeclareOk
    {

        public string _queue;
        public uint _messageCount;
        public uint _consumerCount;

        string IQueueDeclareOk.Queue => _queue;
        uint IQueueDeclareOk.MessageCount => _messageCount;
        uint IQueueDeclareOk.ConsumerCount => _consumerCount;

        public QueueDeclareOk() { }
        public QueueDeclareOk(string @Queue, uint @MessageCount, uint @ConsumerCount)
        {
            _queue = @Queue;
            _messageCount = @MessageCount;
            _consumerCount = @ConsumerCount;
        }

        public override ushort ProtocolClassId => ClassConstants.Queue;
        public override ushort ProtocolMethodId => QueueMethodConstants.DeclareOk;
        public override string ProtocolMethodName => "queue.declare-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _queue = reader.ReadShortstr();
            _messageCount = reader.ReadLong();
            _consumerCount = reader.ReadLong();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShortstr(_queue);
            writer.WriteLong(_messageCount);
            writer.WriteLong(_consumerCount);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_queue); // _queue in bytes
            bufferSize += 4; // _messageCount in bytes
            bufferSize += 4; // _consumerCount in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_queue); sb.Append(",");
            sb.Append(_messageCount); sb.Append(",");
            sb.Append(_consumerCount);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class QueueBind : Client.Impl.MethodBase, IQueueBind
    {

        public ushort _reserved1;
        public string _queue;
        public string _exchange;
        public string _routingKey;
        public bool _nowait;
        public IDictionary<string, object> _arguments;

        ushort IQueueBind.Reserved1 => _reserved1;
        string IQueueBind.Queue => _queue;
        string IQueueBind.Exchange => _exchange;
        string IQueueBind.RoutingKey => _routingKey;
        bool IQueueBind.Nowait => _nowait;
        IDictionary<string, object> IQueueBind.Arguments => _arguments;

        public QueueBind() { }
        public QueueBind(ushort @Reserved1, string @Queue, string @Exchange, string @RoutingKey, bool @Nowait, IDictionary<string, object> @Arguments)
        {
            _reserved1 = @Reserved1;
            _queue = @Queue;
            _exchange = @Exchange;
            _routingKey = @RoutingKey;
            _nowait = @Nowait;
            _arguments = @Arguments;
        }

        public override ushort ProtocolClassId => ClassConstants.Queue;
        public override ushort ProtocolMethodId => QueueMethodConstants.Bind;
        public override string ProtocolMethodName => "queue.bind";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadShort();
            _queue = reader.ReadShortstr();
            _exchange = reader.ReadShortstr();
            _routingKey = reader.ReadShortstr();
            _nowait = reader.ReadBit();
            _arguments = reader.ReadTable();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_reserved1);
            writer.WriteShortstr(_queue);
            writer.WriteShortstr(_exchange);
            writer.WriteShortstr(_routingKey);
            writer.WriteBit(_nowait);
            writer.WriteTable(_arguments);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _reserved1 in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_queue); // _queue in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_exchange); // _exchange in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_routingKey); // _routingKey in bytes
            bufferSize += WireFormatting.GetTableByteCount(_arguments); // _arguments in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1); sb.Append(",");
            sb.Append(_queue); sb.Append(",");
            sb.Append(_exchange); sb.Append(",");
            sb.Append(_routingKey); sb.Append(",");
            sb.Append(_nowait); sb.Append(",");
            sb.Append(_arguments);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class QueueBindOk : Client.Impl.MethodBase, IQueueBindOk
    {



        public QueueBindOk()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Queue;
        public override ushort ProtocolMethodId => QueueMethodConstants.BindOk;
        public override string ProtocolMethodName => "queue.bind-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class QueueUnbind : Client.Impl.MethodBase, IQueueUnbind
    {

        public ushort _reserved1;
        public string _queue;
        public string _exchange;
        public string _routingKey;
        public IDictionary<string, object> _arguments;

        ushort IQueueUnbind.Reserved1 => _reserved1;
        string IQueueUnbind.Queue => _queue;
        string IQueueUnbind.Exchange => _exchange;
        string IQueueUnbind.RoutingKey => _routingKey;
        IDictionary<string, object> IQueueUnbind.Arguments => _arguments;

        public QueueUnbind() { }
        public QueueUnbind(ushort @Reserved1, string @Queue, string @Exchange, string @RoutingKey, IDictionary<string, object> @Arguments)
        {
            _reserved1 = @Reserved1;
            _queue = @Queue;
            _exchange = @Exchange;
            _routingKey = @RoutingKey;
            _arguments = @Arguments;
        }

        public override ushort ProtocolClassId => ClassConstants.Queue;
        public override ushort ProtocolMethodId => QueueMethodConstants.Unbind;
        public override string ProtocolMethodName => "queue.unbind";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadShort();
            _queue = reader.ReadShortstr();
            _exchange = reader.ReadShortstr();
            _routingKey = reader.ReadShortstr();
            _arguments = reader.ReadTable();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_reserved1);
            writer.WriteShortstr(_queue);
            writer.WriteShortstr(_exchange);
            writer.WriteShortstr(_routingKey);
            writer.WriteTable(_arguments);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _reserved1 in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_queue); // _queue in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_exchange); // _exchange in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_routingKey); // _routingKey in bytes
            bufferSize += WireFormatting.GetTableByteCount(_arguments); // _arguments in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1); sb.Append(",");
            sb.Append(_queue); sb.Append(",");
            sb.Append(_exchange); sb.Append(",");
            sb.Append(_routingKey); sb.Append(",");
            sb.Append(_arguments);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class QueueUnbindOk : Client.Impl.MethodBase, IQueueUnbindOk
    {



        public QueueUnbindOk()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Queue;
        public override ushort ProtocolMethodId => QueueMethodConstants.UnbindOk;
        public override string ProtocolMethodName => "queue.unbind-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class QueuePurge : Client.Impl.MethodBase, IQueuePurge
    {

        public ushort _reserved1;
        public string _queue;
        public bool _nowait;

        ushort IQueuePurge.Reserved1 => _reserved1;
        string IQueuePurge.Queue => _queue;
        bool IQueuePurge.Nowait => _nowait;

        public QueuePurge() { }
        public QueuePurge(ushort @Reserved1, string @Queue, bool @Nowait)
        {
            _reserved1 = @Reserved1;
            _queue = @Queue;
            _nowait = @Nowait;
        }

        public override ushort ProtocolClassId => ClassConstants.Queue;
        public override ushort ProtocolMethodId => QueueMethodConstants.Purge;
        public override string ProtocolMethodName => "queue.purge";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadShort();
            _queue = reader.ReadShortstr();
            _nowait = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_reserved1);
            writer.WriteShortstr(_queue);
            writer.WriteBit(_nowait);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _reserved1 in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_queue); // _queue in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1); sb.Append(",");
            sb.Append(_queue); sb.Append(",");
            sb.Append(_nowait);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class QueuePurgeOk : Client.Impl.MethodBase, IQueuePurgeOk
    {

        public uint _messageCount;

        uint IQueuePurgeOk.MessageCount => _messageCount;

        public QueuePurgeOk() { }
        public QueuePurgeOk(uint @MessageCount)
        {
            _messageCount = @MessageCount;
        }

        public override ushort ProtocolClassId => ClassConstants.Queue;
        public override ushort ProtocolMethodId => QueueMethodConstants.PurgeOk;
        public override string ProtocolMethodName => "queue.purge-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _messageCount = reader.ReadLong();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteLong(_messageCount);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 4; // _messageCount in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_messageCount);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class QueueDelete : Client.Impl.MethodBase, IQueueDelete
    {

        public ushort _reserved1;
        public string _queue;
        public bool _ifUnused;
        public bool _ifEmpty;
        public bool _nowait;

        ushort IQueueDelete.Reserved1 => _reserved1;
        string IQueueDelete.Queue => _queue;
        bool IQueueDelete.IfUnused => _ifUnused;
        bool IQueueDelete.IfEmpty => _ifEmpty;
        bool IQueueDelete.Nowait => _nowait;

        public QueueDelete() { }
        public QueueDelete(ushort @Reserved1, string @Queue, bool @IfUnused, bool @IfEmpty, bool @Nowait)
        {
            _reserved1 = @Reserved1;
            _queue = @Queue;
            _ifUnused = @IfUnused;
            _ifEmpty = @IfEmpty;
            _nowait = @Nowait;
        }

        public override ushort ProtocolClassId => ClassConstants.Queue;
        public override ushort ProtocolMethodId => QueueMethodConstants.Delete;
        public override string ProtocolMethodName => "queue.delete";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadShort();
            _queue = reader.ReadShortstr();
            _ifUnused = reader.ReadBit();
            _ifEmpty = reader.ReadBit();
            _nowait = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_reserved1);
            writer.WriteShortstr(_queue);
            writer.WriteBit(_ifUnused);
            writer.WriteBit(_ifEmpty);
            writer.WriteBit(_nowait);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _reserved1 in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_queue); // _queue in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1); sb.Append(",");
            sb.Append(_queue); sb.Append(",");
            sb.Append(_ifUnused); sb.Append(",");
            sb.Append(_ifEmpty); sb.Append(",");
            sb.Append(_nowait);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class QueueDeleteOk : Client.Impl.MethodBase, IQueueDeleteOk
    {

        public uint _messageCount;

        uint IQueueDeleteOk.MessageCount => _messageCount;

        public QueueDeleteOk() { }
        public QueueDeleteOk(uint @MessageCount)
        {
            _messageCount = @MessageCount;
        }

        public override ushort ProtocolClassId => ClassConstants.Queue;
        public override ushort ProtocolMethodId => QueueMethodConstants.DeleteOk;
        public override string ProtocolMethodName => "queue.delete-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _messageCount = reader.ReadLong();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteLong(_messageCount);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 4; // _messageCount in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_messageCount);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicQos : Client.Impl.MethodBase, IBasicQos
    {

        public uint _prefetchSize;
        public ushort _prefetchCount;
        public bool _global;

        uint IBasicQos.PrefetchSize => _prefetchSize;
        ushort IBasicQos.PrefetchCount => _prefetchCount;
        bool IBasicQos.Global => _global;

        public BasicQos() { }
        public BasicQos(uint @PrefetchSize, ushort @PrefetchCount, bool @Global)
        {
            _prefetchSize = @PrefetchSize;
            _prefetchCount = @PrefetchCount;
            _global = @Global;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.Qos;
        public override string ProtocolMethodName => "basic.qos";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _prefetchSize = reader.ReadLong();
            _prefetchCount = reader.ReadShort();
            _global = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteLong(_prefetchSize);
            writer.WriteShort(_prefetchCount);
            writer.WriteBit(_global);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 4; // _prefetchSize in bytes
            bufferSize += 2; // _prefetchCount in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_prefetchSize); sb.Append(",");
            sb.Append(_prefetchCount); sb.Append(",");
            sb.Append(_global);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicQosOk : Client.Impl.MethodBase, IBasicQosOk
    {



        public BasicQosOk()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.QosOk;
        public override string ProtocolMethodName => "basic.qos-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicConsume : Client.Impl.MethodBase, IBasicConsume
    {

        public ushort _reserved1;
        public string _queue;
        public string _consumerTag;
        public bool _noLocal;
        public bool _noAck;
        public bool _exclusive;
        public bool _nowait;
        public IDictionary<string, object> _arguments;

        ushort IBasicConsume.Reserved1 => _reserved1;
        string IBasicConsume.Queue => _queue;
        string IBasicConsume.ConsumerTag => _consumerTag;
        bool IBasicConsume.NoLocal => _noLocal;
        bool IBasicConsume.NoAck => _noAck;
        bool IBasicConsume.Exclusive => _exclusive;
        bool IBasicConsume.Nowait => _nowait;
        IDictionary<string, object> IBasicConsume.Arguments => _arguments;

        public BasicConsume() { }
        public BasicConsume(ushort @Reserved1, string @Queue, string @ConsumerTag, bool @NoLocal, bool @NoAck, bool @Exclusive, bool @Nowait, IDictionary<string, object> @Arguments)
        {
            _reserved1 = @Reserved1;
            _queue = @Queue;
            _consumerTag = @ConsumerTag;
            _noLocal = @NoLocal;
            _noAck = @NoAck;
            _exclusive = @Exclusive;
            _nowait = @Nowait;
            _arguments = @Arguments;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.Consume;
        public override string ProtocolMethodName => "basic.consume";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadShort();
            _queue = reader.ReadShortstr();
            _consumerTag = reader.ReadShortstr();
            _noLocal = reader.ReadBit();
            _noAck = reader.ReadBit();
            _exclusive = reader.ReadBit();
            _nowait = reader.ReadBit();
            _arguments = reader.ReadTable();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_reserved1);
            writer.WriteShortstr(_queue);
            writer.WriteShortstr(_consumerTag);
            writer.WriteBit(_noLocal);
            writer.WriteBit(_noAck);
            writer.WriteBit(_exclusive);
            writer.WriteBit(_nowait);
            writer.WriteTable(_arguments);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _reserved1 in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_queue); // _queue in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_consumerTag); // _consumerTag in bytes
            bufferSize += WireFormatting.GetTableByteCount(_arguments); // _arguments in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1); sb.Append(",");
            sb.Append(_queue); sb.Append(",");
            sb.Append(_consumerTag); sb.Append(",");
            sb.Append(_noLocal); sb.Append(",");
            sb.Append(_noAck); sb.Append(",");
            sb.Append(_exclusive); sb.Append(",");
            sb.Append(_nowait); sb.Append(",");
            sb.Append(_arguments);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicConsumeOk : Client.Impl.MethodBase, IBasicConsumeOk
    {

        public string _consumerTag;

        string IBasicConsumeOk.ConsumerTag => _consumerTag;

        public BasicConsumeOk() { }
        public BasicConsumeOk(string @ConsumerTag)
        {
            _consumerTag = @ConsumerTag;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.ConsumeOk;
        public override string ProtocolMethodName => "basic.consume-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _consumerTag = reader.ReadShortstr();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShortstr(_consumerTag);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_consumerTag); // _consumerTag in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_consumerTag);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicCancel : Client.Impl.MethodBase, IBasicCancel
    {

        public string _consumerTag;
        public bool _nowait;

        string IBasicCancel.ConsumerTag => _consumerTag;
        bool IBasicCancel.Nowait => _nowait;

        public BasicCancel() { }
        public BasicCancel(string @ConsumerTag, bool @Nowait)
        {
            _consumerTag = @ConsumerTag;
            _nowait = @Nowait;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.Cancel;
        public override string ProtocolMethodName => "basic.cancel";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _consumerTag = reader.ReadShortstr();
            _nowait = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShortstr(_consumerTag);
            writer.WriteBit(_nowait);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_consumerTag); // _consumerTag in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_consumerTag); sb.Append(",");
            sb.Append(_nowait);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicCancelOk : Client.Impl.MethodBase, IBasicCancelOk
    {

        public string _consumerTag;

        string IBasicCancelOk.ConsumerTag => _consumerTag;

        public BasicCancelOk() { }
        public BasicCancelOk(string @ConsumerTag)
        {
            _consumerTag = @ConsumerTag;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.CancelOk;
        public override string ProtocolMethodName => "basic.cancel-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _consumerTag = reader.ReadShortstr();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShortstr(_consumerTag);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_consumerTag); // _consumerTag in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_consumerTag);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicPublish : Client.Impl.MethodBase, IBasicPublish
    {

        public ushort _reserved1;
        public string _exchange;
        public string _routingKey;
        public bool _mandatory;
        public bool _immediate;

        ushort IBasicPublish.Reserved1 => _reserved1;
        string IBasicPublish.Exchange => _exchange;
        string IBasicPublish.RoutingKey => _routingKey;
        bool IBasicPublish.Mandatory => _mandatory;
        bool IBasicPublish.Immediate => _immediate;

        public BasicPublish() { }
        public BasicPublish(ushort @Reserved1, string @Exchange, string @RoutingKey, bool @Mandatory, bool @Immediate)
        {
            _reserved1 = @Reserved1;
            _exchange = @Exchange;
            _routingKey = @RoutingKey;
            _mandatory = @Mandatory;
            _immediate = @Immediate;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.Publish;
        public override string ProtocolMethodName => "basic.publish";
        public override bool HasContent => true;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadShort();
            _exchange = reader.ReadShortstr();
            _routingKey = reader.ReadShortstr();
            _mandatory = reader.ReadBit();
            _immediate = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_reserved1);
            writer.WriteShortstr(_exchange);
            writer.WriteShortstr(_routingKey);
            writer.WriteBit(_mandatory);
            writer.WriteBit(_immediate);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _reserved1 in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_exchange); // _exchange in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_routingKey); // _routingKey in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1); sb.Append(",");
            sb.Append(_exchange); sb.Append(",");
            sb.Append(_routingKey); sb.Append(",");
            sb.Append(_mandatory); sb.Append(",");
            sb.Append(_immediate);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicReturn : Client.Impl.MethodBase, IBasicReturn
    {

        public ushort _replyCode;
        public string _replyText;
        public string _exchange;
        public string _routingKey;

        ushort IBasicReturn.ReplyCode => _replyCode;
        string IBasicReturn.ReplyText => _replyText;
        string IBasicReturn.Exchange => _exchange;
        string IBasicReturn.RoutingKey => _routingKey;

        public BasicReturn() { }
        public BasicReturn(ushort @ReplyCode, string @ReplyText, string @Exchange, string @RoutingKey)
        {
            _replyCode = @ReplyCode;
            _replyText = @ReplyText;
            _exchange = @Exchange;
            _routingKey = @RoutingKey;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.Return;
        public override string ProtocolMethodName => "basic.return";
        public override bool HasContent => true;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _replyCode = reader.ReadShort();
            _replyText = reader.ReadShortstr();
            _exchange = reader.ReadShortstr();
            _routingKey = reader.ReadShortstr();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_replyCode);
            writer.WriteShortstr(_replyText);
            writer.WriteShortstr(_exchange);
            writer.WriteShortstr(_routingKey);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _replyCode in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_replyText); // _replyText in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_exchange); // _exchange in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_routingKey); // _routingKey in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_replyCode); sb.Append(",");
            sb.Append(_replyText); sb.Append(",");
            sb.Append(_exchange); sb.Append(",");
            sb.Append(_routingKey);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicDeliver : Client.Impl.MethodBase, IBasicDeliver
    {

        public string _consumerTag;
        public ulong _deliveryTag;
        public bool _redelivered;
        public string _exchange;
        public string _routingKey;

        string IBasicDeliver.ConsumerTag => _consumerTag;
        ulong IBasicDeliver.DeliveryTag => _deliveryTag;
        bool IBasicDeliver.Redelivered => _redelivered;
        string IBasicDeliver.Exchange => _exchange;
        string IBasicDeliver.RoutingKey => _routingKey;

        public BasicDeliver() { }
        public BasicDeliver(string @ConsumerTag, ulong @DeliveryTag, bool @Redelivered, string @Exchange, string @RoutingKey)
        {
            _consumerTag = @ConsumerTag;
            _deliveryTag = @DeliveryTag;
            _redelivered = @Redelivered;
            _exchange = @Exchange;
            _routingKey = @RoutingKey;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.Deliver;
        public override string ProtocolMethodName => "basic.deliver";
        public override bool HasContent => true;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _consumerTag = reader.ReadShortstr();
            _deliveryTag = reader.ReadLonglong();
            _redelivered = reader.ReadBit();
            _exchange = reader.ReadShortstr();
            _routingKey = reader.ReadShortstr();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShortstr(_consumerTag);
            writer.WriteLonglong(_deliveryTag);
            writer.WriteBit(_redelivered);
            writer.WriteShortstr(_exchange);
            writer.WriteShortstr(_routingKey);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_consumerTag); // _consumerTag in bytes
            bufferSize += 8; // _deliveryTag in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_exchange); // _exchange in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_routingKey); // _routingKey in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_consumerTag); sb.Append(",");
            sb.Append(_deliveryTag); sb.Append(",");
            sb.Append(_redelivered); sb.Append(",");
            sb.Append(_exchange); sb.Append(",");
            sb.Append(_routingKey);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicGet : Client.Impl.MethodBase, IBasicGet
    {

        public ushort _reserved1;
        public string _queue;
        public bool _noAck;

        ushort IBasicGet.Reserved1 => _reserved1;
        string IBasicGet.Queue => _queue;
        bool IBasicGet.NoAck => _noAck;

        public BasicGet() { }
        public BasicGet(ushort @Reserved1, string @Queue, bool @NoAck)
        {
            _reserved1 = @Reserved1;
            _queue = @Queue;
            _noAck = @NoAck;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.Get;
        public override string ProtocolMethodName => "basic.get";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadShort();
            _queue = reader.ReadShortstr();
            _noAck = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShort(_reserved1);
            writer.WriteShortstr(_queue);
            writer.WriteBit(_noAck);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 2; // _reserved1 in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_queue); // _queue in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1); sb.Append(",");
            sb.Append(_queue); sb.Append(",");
            sb.Append(_noAck);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicGetOk : Client.Impl.MethodBase, IBasicGetOk
    {

        public ulong _deliveryTag;
        public bool _redelivered;
        public string _exchange;
        public string _routingKey;
        public uint _messageCount;

        ulong IBasicGetOk.DeliveryTag => _deliveryTag;
        bool IBasicGetOk.Redelivered => _redelivered;
        string IBasicGetOk.Exchange => _exchange;
        string IBasicGetOk.RoutingKey => _routingKey;
        uint IBasicGetOk.MessageCount => _messageCount;

        public BasicGetOk() { }
        public BasicGetOk(ulong @DeliveryTag, bool @Redelivered, string @Exchange, string @RoutingKey, uint @MessageCount)
        {
            _deliveryTag = @DeliveryTag;
            _redelivered = @Redelivered;
            _exchange = @Exchange;
            _routingKey = @RoutingKey;
            _messageCount = @MessageCount;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.GetOk;
        public override string ProtocolMethodName => "basic.get-ok";
        public override bool HasContent => true;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _deliveryTag = reader.ReadLonglong();
            _redelivered = reader.ReadBit();
            _exchange = reader.ReadShortstr();
            _routingKey = reader.ReadShortstr();
            _messageCount = reader.ReadLong();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteLonglong(_deliveryTag);
            writer.WriteBit(_redelivered);
            writer.WriteShortstr(_exchange);
            writer.WriteShortstr(_routingKey);
            writer.WriteLong(_messageCount);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 8; // _deliveryTag in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_exchange); // _exchange in bytes
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_routingKey); // _routingKey in bytes
            bufferSize += 4; // _messageCount in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_deliveryTag); sb.Append(",");
            sb.Append(_redelivered); sb.Append(",");
            sb.Append(_exchange); sb.Append(",");
            sb.Append(_routingKey); sb.Append(",");
            sb.Append(_messageCount);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicGetEmpty : Client.Impl.MethodBase, IBasicGetEmpty
    {

        public string _reserved1;

        string IBasicGetEmpty.Reserved1 => _reserved1;

        public BasicGetEmpty() { }
        public BasicGetEmpty(string @Reserved1)
        {
            _reserved1 = @Reserved1;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.GetEmpty;
        public override string ProtocolMethodName => "basic.get-empty";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _reserved1 = reader.ReadShortstr();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteShortstr(_reserved1);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 1 + Encoding.UTF8.GetByteCount(_reserved1); // _reserved1 in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_reserved1);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicAck : Client.Impl.MethodBase, IBasicAck
    {

        public ulong _deliveryTag;
        public bool _multiple;

        ulong IBasicAck.DeliveryTag => _deliveryTag;
        bool IBasicAck.Multiple => _multiple;

        public BasicAck() { }
        public BasicAck(ulong @DeliveryTag, bool @Multiple)
        {
            _deliveryTag = @DeliveryTag;
            _multiple = @Multiple;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.Ack;
        public override string ProtocolMethodName => "basic.ack";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _deliveryTag = reader.ReadLonglong();
            _multiple = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteLonglong(_deliveryTag);
            writer.WriteBit(_multiple);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 8; // _deliveryTag in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_deliveryTag); sb.Append(",");
            sb.Append(_multiple);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicReject : Client.Impl.MethodBase, IBasicReject
    {

        public ulong _deliveryTag;
        public bool _requeue;

        ulong IBasicReject.DeliveryTag => _deliveryTag;
        bool IBasicReject.Requeue => _requeue;

        public BasicReject() { }
        public BasicReject(ulong @DeliveryTag, bool @Requeue)
        {
            _deliveryTag = @DeliveryTag;
            _requeue = @Requeue;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.Reject;
        public override string ProtocolMethodName => "basic.reject";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _deliveryTag = reader.ReadLonglong();
            _requeue = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteLonglong(_deliveryTag);
            writer.WriteBit(_requeue);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 8; // _deliveryTag in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_deliveryTag); sb.Append(",");
            sb.Append(_requeue);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicRecoverAsync : Client.Impl.MethodBase, IBasicRecoverAsync
    {

        public bool _requeue;

        bool IBasicRecoverAsync.Requeue => _requeue;

        public BasicRecoverAsync() { }
        public BasicRecoverAsync(bool @Requeue)
        {
            _requeue = @Requeue;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.RecoverAsync;
        public override string ProtocolMethodName => "basic.recover-async";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _requeue = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteBit(_requeue);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_requeue);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicRecover : Client.Impl.MethodBase, IBasicRecover
    {

        public bool _requeue;

        bool IBasicRecover.Requeue => _requeue;

        public BasicRecover() { }
        public BasicRecover(bool @Requeue)
        {
            _requeue = @Requeue;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.Recover;
        public override string ProtocolMethodName => "basic.recover";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _requeue = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteBit(_requeue);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_requeue);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicRecoverOk : Client.Impl.MethodBase, IBasicRecoverOk
    {



        public BasicRecoverOk()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.RecoverOk;
        public override string ProtocolMethodName => "basic.recover-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class BasicNack : Client.Impl.MethodBase, IBasicNack
    {

        public ulong _deliveryTag;
        public bool _multiple;
        public bool _requeue;

        ulong IBasicNack.DeliveryTag => _deliveryTag;
        bool IBasicNack.Multiple => _multiple;
        bool IBasicNack.Requeue => _requeue;

        public BasicNack() { }
        public BasicNack(ulong @DeliveryTag, bool @Multiple, bool @Requeue)
        {
            _deliveryTag = @DeliveryTag;
            _multiple = @Multiple;
            _requeue = @Requeue;
        }

        public override ushort ProtocolClassId => ClassConstants.Basic;
        public override ushort ProtocolMethodId => BasicMethodConstants.Nack;
        public override string ProtocolMethodName => "basic.nack";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _deliveryTag = reader.ReadLonglong();
            _multiple = reader.ReadBit();
            _requeue = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteLonglong(_deliveryTag);
            writer.WriteBit(_multiple);
            writer.WriteBit(_requeue);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 8; // _deliveryTag in bytes
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_deliveryTag); sb.Append(",");
            sb.Append(_multiple); sb.Append(",");
            sb.Append(_requeue);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class TxSelect : Client.Impl.MethodBase, ITxSelect
    {



        public TxSelect()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Tx;
        public override ushort ProtocolMethodId => TxMethodConstants.Select;
        public override string ProtocolMethodName => "tx.select";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class TxSelectOk : Client.Impl.MethodBase, ITxSelectOk
    {



        public TxSelectOk()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Tx;
        public override ushort ProtocolMethodId => TxMethodConstants.SelectOk;
        public override string ProtocolMethodName => "tx.select-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class TxCommit : Client.Impl.MethodBase, ITxCommit
    {



        public TxCommit()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Tx;
        public override ushort ProtocolMethodId => TxMethodConstants.Commit;
        public override string ProtocolMethodName => "tx.commit";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class TxCommitOk : Client.Impl.MethodBase, ITxCommitOk
    {



        public TxCommitOk()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Tx;
        public override ushort ProtocolMethodId => TxMethodConstants.CommitOk;
        public override string ProtocolMethodName => "tx.commit-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class TxRollback : Client.Impl.MethodBase, ITxRollback
    {



        public TxRollback()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Tx;
        public override ushort ProtocolMethodId => TxMethodConstants.Rollback;
        public override string ProtocolMethodName => "tx.rollback";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class TxRollbackOk : Client.Impl.MethodBase, ITxRollbackOk
    {



        public TxRollbackOk()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Tx;
        public override ushort ProtocolMethodId => TxMethodConstants.RollbackOk;
        public override string ProtocolMethodName => "tx.rollback-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConfirmSelect : Client.Impl.MethodBase, IConfirmSelect
    {

        public bool _nowait;

        bool IConfirmSelect.Nowait => _nowait;

        public ConfirmSelect() { }
        public ConfirmSelect(bool @Nowait)
        {
            _nowait = @Nowait;
        }

        public override ushort ProtocolClassId => ClassConstants.Confirm;
        public override ushort ProtocolMethodId => ConfirmMethodConstants.Select;
        public override string ProtocolMethodName => "confirm.select";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
            _nowait = reader.ReadBit();
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
            writer.WriteBit(_nowait);
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            bufferSize += 1; // number of bit fields in bytes
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(_nowait);
            sb.Append(")");
        }
    }
    /// <summary>Autogenerated type. Private implementation class - do not use directly.</summary>
    internal sealed class ConfirmSelectOk : Client.Impl.MethodBase, IConfirmSelectOk
    {



        public ConfirmSelectOk()
        {
        }

        public override ushort ProtocolClassId => ClassConstants.Confirm;
        public override ushort ProtocolMethodId => ConfirmMethodConstants.SelectOk;
        public override string ProtocolMethodName => "confirm.select-ok";
        public override bool HasContent => false;

        public override void ReadArgumentsFrom(ref Client.Impl.MethodArgumentReader reader)
        {
        }

        public override void WriteArgumentsTo(ref Client.Impl.MethodArgumentWriter writer)
        {
        }

        public override int GetRequiredBufferSize()
        {
            int bufferSize = 0;
            return bufferSize;
        }

        public override void AppendArgumentDebugStringTo(StringBuilder sb)
        {
            sb.Append("(");
            sb.Append(")");
        }
    }

    internal class Model : Client.Impl.ModelBase
    {
        public Model(Client.Impl.ISession session) : base(session) { }
        public Model(Client.Impl.ISession session, ConsumerWorkService workService) : base(session, workService) { }
        public override void ConnectionTuneOk(ushort @channelMax, uint @frameMax, ushort @heartbeat)
        {
            ConnectionTuneOk __req = new ConnectionTuneOk()
            {
                _channelMax = @channelMax,
                _frameMax = @frameMax,
                _heartbeat = @heartbeat,
            };
            ModelSend(__req, null, null);
        }
        public override void _Private_BasicCancel(string @consumerTag, bool @nowait)
        {
            BasicCancel __req = new BasicCancel()
            {
                _consumerTag = @consumerTag,
                _nowait = @nowait,
            };
            ModelSend(__req, null, null);
        }
        public override void _Private_BasicConsume(string @queue, string @consumerTag, bool @noLocal, bool @autoAck, bool @exclusive, bool @nowait, IDictionary<string, object> @arguments)
        {
            BasicConsume __req = new BasicConsume()
            {
                _queue = @queue,
                _consumerTag = @consumerTag,
                _noLocal = @noLocal,
                _noAck = @autoAck,
                _exclusive = @exclusive,
                _nowait = @nowait,
                _arguments = @arguments,
            };
            ModelSend(__req, null, null);
        }
        public override void _Private_BasicGet(string @queue, bool @autoAck)
        {
            BasicGet __req = new BasicGet()
            {
                _queue = @queue,
                _noAck = @autoAck,
            };
            ModelSend(__req, null, null);
        }
        public override void _Private_BasicPublish(string @exchange, string @routingKey, bool @mandatory, RabbitMQ.Client.IBasicProperties @basicProperties, ReadOnlyMemory<byte> @body)
        {
            BasicPublish __req = new BasicPublish()
            {
                _exchange = @exchange,
                _routingKey = @routingKey,
                _mandatory = @mandatory,
            };
            ModelSend(__req, (BasicProperties)basicProperties, body);
        }
        public override void _Private_BasicRecover(bool @requeue)
        {
            BasicRecover __req = new BasicRecover()
            {
                _requeue = @requeue,
            };
            ModelSend(__req, null, null);
        }
        public override void _Private_ChannelClose(ushort @replyCode, string @replyText, ushort @classId, ushort @methodId)
        {
            ChannelClose __req = new ChannelClose()
            {
                _replyCode = @replyCode,
                _replyText = @replyText,
                _classId = @classId,
                _methodId = @methodId,
            };
            ModelSend(__req, null, null);
        }
        public override void _Private_ChannelCloseOk()
        {
            ChannelCloseOk __req = new ChannelCloseOk();
            ModelSend(__req, null, null);
        }
        public override void _Private_ChannelFlowOk(bool @active)
        {
            ChannelFlowOk __req = new ChannelFlowOk()
            {
                _active = @active,
            };
            ModelSend(__req, null, null);
        }
        public override void _Private_ChannelOpen(string @outOfBand)
        {
            ChannelOpen __req = new ChannelOpen()
            {
                _reserved1 = @outOfBand,
            };
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is ChannelOpenOk))
            {
                throw new UnexpectedMethodException(__repBase);
            }
        }
        public override void _Private_ConfirmSelect(bool @nowait)
        {
            ConfirmSelect __req = new ConfirmSelect()
            {
                _nowait = @nowait,
            };
            if (nowait)
            {
                ModelSend(__req, null, null);
                return;
            }
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is ConfirmSelectOk))
            {
                throw new UnexpectedMethodException(__repBase);
            }
        }
        public override void _Private_ConnectionClose(ushort @replyCode, string @replyText, ushort @classId, ushort @methodId)
        {
            ConnectionClose __req = new ConnectionClose()
            {
                _replyCode = @replyCode,
                _replyText = @replyText,
                _classId = @classId,
                _methodId = @methodId,
            };
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is ConnectionCloseOk))
            {
                throw new UnexpectedMethodException(__repBase);
            }
        }
        public override void _Private_ConnectionCloseOk()
        {
            ConnectionCloseOk __req = new ConnectionCloseOk();
            ModelSend(__req, null, null);
        }
        public override void _Private_ConnectionOpen(string @virtualHost, string @capabilities, bool @insist)
        {
            ConnectionOpen __req = new ConnectionOpen()
            {
                _virtualHost = @virtualHost,
                _reserved1 = @capabilities,
                _reserved2 = @insist,
            };
            ModelSend(__req, null, null);
        }
        public override void _Private_ConnectionSecureOk(byte[] @response)
        {
            ConnectionSecureOk __req = new ConnectionSecureOk()
            {
                _response = @response,
            };
            ModelSend(__req, null, null);
        }
        public override void _Private_ConnectionStartOk(IDictionary<string, object> @clientProperties, string @mechanism, byte[] @response, string @locale)
        {
            ConnectionStartOk __req = new ConnectionStartOk()
            {
                _clientProperties = @clientProperties,
                _mechanism = @mechanism,
                _response = @response,
                _locale = @locale,
            };
            ModelSend(__req, null, null);
        }
        public override void _Private_UpdateSecret(byte[] @newSecret, string @reason)
        {
            ConnectionUpdateSecret __req = new ConnectionUpdateSecret()
            {
                _newSecret = @newSecret,
                _reason = @reason,
            };
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is ConnectionUpdateSecretOk))
            {
                throw new UnexpectedMethodException(__repBase);
            }
        }
        public override void _Private_ExchangeBind(string @destination, string @source, string @routingKey, bool @nowait, IDictionary<string, object> @arguments)
        {
            ExchangeBind __req = new ExchangeBind()
            {
                _destination = @destination,
                _source = @source,
                _routingKey = @routingKey,
                _nowait = @nowait,
                _arguments = @arguments,
            };
            if (nowait)
            {
                ModelSend(__req, null, null);
                return;
            }
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is ExchangeBindOk))
            {
                throw new UnexpectedMethodException(__repBase);
            }
        }
        public override void _Private_ExchangeDeclare(string @exchange, string @type, bool @passive, bool @durable, bool @autoDelete, bool @internal, bool @nowait, IDictionary<string, object> @arguments)
        {
            ExchangeDeclare __req = new ExchangeDeclare()
            {
                _exchange = @exchange,
                _type = @type,
                _passive = @passive,
                _durable = @durable,
                _autoDelete = @autoDelete,
                _internal = @internal,
                _nowait = @nowait,
                _arguments = @arguments,
            };
            if (nowait)
            {
                ModelSend(__req, null, null);
                return;
            }
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is ExchangeDeclareOk))
            {
                throw new UnexpectedMethodException(__repBase);
            }
        }
        public override void _Private_ExchangeDelete(string @exchange, bool @ifUnused, bool @nowait)
        {
            ExchangeDelete __req = new ExchangeDelete()
            {
                _exchange = @exchange,
                _ifUnused = @ifUnused,
                _nowait = @nowait,
            };
            if (nowait)
            {
                ModelSend(__req, null, null);
                return;
            }
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is ExchangeDeleteOk))
            {
                throw new UnexpectedMethodException(__repBase);
            }
        }
        public override void _Private_ExchangeUnbind(string @destination, string @source, string @routingKey, bool @nowait, IDictionary<string, object> @arguments)
        {
            ExchangeUnbind __req = new ExchangeUnbind()
            {
                _destination = @destination,
                _source = @source,
                _routingKey = @routingKey,
                _nowait = @nowait,
                _arguments = @arguments,
            };
            if (nowait)
            {
                ModelSend(__req, null, null);
                return;
            }
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is ExchangeUnbindOk))
            {
                throw new UnexpectedMethodException(__repBase);
            }
        }
        public override void _Private_QueueBind(string @queue, string @exchange, string @routingKey, bool @nowait, IDictionary<string, object> @arguments)
        {
            QueueBind __req = new QueueBind()
            {
                _queue = @queue,
                _exchange = @exchange,
                _routingKey = @routingKey,
                _nowait = @nowait,
                _arguments = @arguments,
            };
            if (nowait)
            {
                ModelSend(__req, null, null);
                return;
            }
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is QueueBindOk))
            {
                throw new UnexpectedMethodException(__repBase);
            }
        }
        public override void _Private_QueueDeclare(string @queue, bool @passive, bool @durable, bool @exclusive, bool @autoDelete, bool @nowait, IDictionary<string, object> @arguments)
        {
            QueueDeclare __req = new QueueDeclare()
            {
                _queue = @queue,
                _passive = @passive,
                _durable = @durable,
                _exclusive = @exclusive,
                _autoDelete = @autoDelete,
                _nowait = @nowait,
                _arguments = @arguments,
            };
            if (nowait)
            {
                ModelSend(__req, null, null);
                return;
            }
            ModelSend(__req, null, null);
        }
        public override uint _Private_QueueDelete(string @queue, bool @ifUnused, bool @ifEmpty, bool @nowait)
        {
            QueueDelete __req = new QueueDelete()
            {
                _queue = @queue,
                _ifUnused = @ifUnused,
                _ifEmpty = @ifEmpty,
                _nowait = @nowait,
            };
            if (nowait)
            {
                ModelSend(__req, null, null);
                return 0xFFFFFFFF;
            }
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is QueueDeleteOk __rep))
            {
                throw new UnexpectedMethodException(__repBase);
            }
            return __rep._messageCount;
        }
        public override uint _Private_QueuePurge(string @queue, bool @nowait)
        {
            QueuePurge __req = new QueuePurge()
            {
                _queue = @queue,
                _nowait = @nowait,
            };
            if (nowait)
            {
                ModelSend(__req, null, null);
                return 0xFFFFFFFF;
            }
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is QueuePurgeOk __rep))
            {
                throw new UnexpectedMethodException(__repBase);
            }
            return __rep._messageCount;
        }
        public override void BasicAck(ulong @deliveryTag, bool @multiple)
        {
            BasicAck __req = new BasicAck()
            {
                _deliveryTag = @deliveryTag,
                _multiple = @multiple,
            };
            ModelSend(__req, null, null);
        }
        public override void BasicNack(ulong @deliveryTag, bool @multiple, bool @requeue)
        {
            BasicNack __req = new BasicNack()
            {
                _deliveryTag = @deliveryTag,
                _multiple = @multiple,
                _requeue = @requeue,
            };
            ModelSend(__req, null, null);
        }
        public override void BasicQos(uint @prefetchSize, ushort @prefetchCount, bool @global)
        {
            BasicQos __req = new BasicQos()
            {
                _prefetchSize = @prefetchSize,
                _prefetchCount = @prefetchCount,
                _global = @global,
            };
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is BasicQosOk))
            {
                throw new UnexpectedMethodException(__repBase);
            }
        }
        public override void BasicRecoverAsync(bool @requeue)
        {
            BasicRecoverAsync __req = new BasicRecoverAsync()
            {
                _requeue = @requeue,
            };
            ModelSend(__req, null, null);
        }
        public override void BasicReject(ulong @deliveryTag, bool @requeue)
        {
            BasicReject __req = new BasicReject()
            {
                _deliveryTag = @deliveryTag,
                _requeue = @requeue,
            };
            ModelSend(__req, null, null);
        }
        public override RabbitMQ.Client.IBasicProperties CreateBasicProperties()
        {
            return new BasicProperties();
        }
        public override void QueueUnbind(string @queue, string @exchange, string @routingKey, IDictionary<string, object> @arguments)
        {
            QueueUnbind __req = new QueueUnbind()
            {
                _queue = @queue,
                _exchange = @exchange,
                _routingKey = @routingKey,
                _arguments = @arguments,
            };
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is QueueUnbindOk))
            {
                throw new UnexpectedMethodException(__repBase);
            }
        }
        public override void TxCommit()
        {
            TxCommit __req = new TxCommit();
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is TxCommitOk))
            {
                throw new UnexpectedMethodException(__repBase);
            }
        }
        public override void TxRollback()
        {
            TxRollback __req = new TxRollback();
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is TxRollbackOk))
            {
                throw new UnexpectedMethodException(__repBase);
            }
        }
        public override void TxSelect()
        {
            TxSelect __req = new TxSelect();
            Client.Impl.MethodBase __repBase = ModelRpc(__req, null, null);
            if (!(__repBase is TxSelectOk))
            {
                throw new UnexpectedMethodException(__repBase);
            }
        }
        public override bool DispatchAsynchronous(Client.Impl.Command cmd)
        {
            switch ((cmd.Method.ProtocolClassId << 16) | cmd.Method.ProtocolMethodId)
            {
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Ack:
                    {
                        var __impl = (BasicAck)cmd.Method;
                        HandleBasicAck(__impl._deliveryTag, __impl._multiple);
                        return true;
                    }
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Cancel:
                    {
                        var __impl = (BasicCancel)cmd.Method;
                        HandleBasicCancel(__impl._consumerTag, __impl._nowait);
                        return true;
                    }
                case (ClassConstants.Basic << 16) | BasicMethodConstants.CancelOk:
                    {
                        var __impl = (BasicCancelOk)cmd.Method;
                        HandleBasicCancelOk(__impl._consumerTag);
                        return true;
                    }
                case (ClassConstants.Basic << 16) | BasicMethodConstants.ConsumeOk:
                    {
                        var __impl = (BasicConsumeOk)cmd.Method;
                        HandleBasicConsumeOk(__impl._consumerTag);
                        return true;
                    }
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Deliver:
                    {
                        var __impl = (BasicDeliver)cmd.Method;
                        HandleBasicDeliver(__impl._consumerTag, __impl._deliveryTag, __impl._redelivered, __impl._exchange, __impl._routingKey, (RabbitMQ.Client.IBasicProperties)cmd.Header, cmd.Body);
                        return true;
                    }
                case (ClassConstants.Basic << 16) | BasicMethodConstants.GetEmpty:
                    {
                        HandleBasicGetEmpty();
                        return true;
                    }
                case (ClassConstants.Basic << 16) | BasicMethodConstants.GetOk:
                    {
                        var __impl = (BasicGetOk)cmd.Method;
                        HandleBasicGetOk(__impl._deliveryTag, __impl._redelivered, __impl._exchange, __impl._routingKey, __impl._messageCount, (RabbitMQ.Client.IBasicProperties)cmd.Header, cmd.Body);
                        return true;
                    }
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Nack:
                    {
                        var __impl = (BasicNack)cmd.Method;
                        HandleBasicNack(__impl._deliveryTag, __impl._multiple, __impl._requeue);
                        return true;
                    }
                case (ClassConstants.Basic << 16) | BasicMethodConstants.RecoverOk:
                    {
                        HandleBasicRecoverOk();
                        return true;
                    }
                case (ClassConstants.Basic << 16) | BasicMethodConstants.Return:
                    {
                        var __impl = (BasicReturn)cmd.Method;
                        HandleBasicReturn(__impl._replyCode, __impl._replyText, __impl._exchange, __impl._routingKey, (RabbitMQ.Client.IBasicProperties)cmd.Header, cmd.Body);
                        return true;
                    }
                case (ClassConstants.Channel << 16) | ChannelMethodConstants.Close:
                    {
                        var __impl = (ChannelClose)cmd.Method;
                        HandleChannelClose(__impl._replyCode, __impl._replyText, __impl._classId, __impl._methodId);
                        return true;
                    }
                case (ClassConstants.Channel << 16) | ChannelMethodConstants.CloseOk:
                    {
                        HandleChannelCloseOk();
                        return true;
                    }
                case (ClassConstants.Channel << 16) | ChannelMethodConstants.Flow:
                    {
                        var __impl = (ChannelFlow)cmd.Method;
                        HandleChannelFlow(__impl._active);
                        return true;
                    }
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.Blocked:
                    {
                        var __impl = (ConnectionBlocked)cmd.Method;
                        HandleConnectionBlocked(__impl._reason);
                        return true;
                    }
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.Close:
                    {
                        var __impl = (ConnectionClose)cmd.Method;
                        HandleConnectionClose(__impl._replyCode, __impl._replyText, __impl._classId, __impl._methodId);
                        return true;
                    }
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.OpenOk:
                    {
                        var __impl = (ConnectionOpenOk)cmd.Method;
                        HandleConnectionOpenOk(__impl._reserved1);
                        return true;
                    }
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.Secure:
                    {
                        var __impl = (ConnectionSecure)cmd.Method;
                        HandleConnectionSecure(__impl._challenge);
                        return true;
                    }
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.Start:
                    {
                        var __impl = (ConnectionStart)cmd.Method;
                        HandleConnectionStart(__impl._versionMajor, __impl._versionMinor, __impl._serverProperties, __impl._mechanisms, __impl._locales);
                        return true;
                    }
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.Tune:
                    {
                        var __impl = (ConnectionTune)cmd.Method;
                        HandleConnectionTune(__impl._channelMax, __impl._frameMax, __impl._heartbeat);
                        return true;
                    }
                case (ClassConstants.Connection << 16) | ConnectionMethodConstants.Unblocked:
                    {
                        HandleConnectionUnblocked();
                        return true;
                    }
                case (ClassConstants.Queue << 16) | QueueMethodConstants.DeclareOk:
                    {
                        var __impl = (QueueDeclareOk)cmd.Method;
                        HandleQueueDeclareOk(__impl._queue, __impl._messageCount, __impl._consumerCount);
                        return true;
                    }
                default: return false;
            }
        }
    }
}
