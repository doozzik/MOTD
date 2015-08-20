using System.Xml.Serialization;

namespace MOTD
{
    public class Message
    {
        [XmlAttribute("Text")]
        public string text;

        [XmlAttribute("Color")]
        public string color;

        public Message() { }

        public Message(string text, string color)
        {
            this.text = text;
            this.color = color;
        }
    }
}
