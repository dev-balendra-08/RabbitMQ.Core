using RabbitMQ.Client.Framing;

namespace RabbitMQ.Client.Exceptions
{
    /// <summary> Thrown when the server sends a frame along a channel
    /// that we do not currently have a Session entry in our
    /// SessionManager for. </summary>
    public class ChannelErrorException : HardProtocolException
    {
        public ChannelErrorException(int channel)
            : base($"Frame received for invalid channel {channel}")
        {
            Channel = channel;
        }

        protected ChannelErrorException(string message) : base(message)
        {
        }

        protected ChannelErrorException() : base()
        {
        }

        protected ChannelErrorException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        ///<summary>The channel number concerned.</summary>
        public int Channel { get; }

        public override ushort ReplyCode
        {
            get { return Constants.ChannelError; }
        }
    }
}
