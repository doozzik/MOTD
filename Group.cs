using System.Collections.Generic;

namespace MOTD
{
    public class Group
    {
        public string Name;
        public List<Message> Messages;

        public Group() { }

        public Group(string name, List<Message> Messages)
        {
            this.Name = name;
            this.Messages = Messages;
        }
    }
}
