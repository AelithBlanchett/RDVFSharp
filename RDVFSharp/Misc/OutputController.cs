using System.Collections.Generic;

namespace RDVFSharp
{
    public class OutputController
    {
        public List<string> Action { get; set; } = new List<string>();
        public List<string> Hit { get; set; } = new List<string>();
        public List<string> Status { get; set; } = new List<string>();
        public List<string> Hint { get; set; } = new List<string>();
        public List<string> Special { get; set; } = new List<string>();
        public List<string> Info { get; set; } = new List<string>();
        public List<string> Error { get; set; } = new List<string>();

        public int Damage { get; set; } = 0;

        public string LastMessageSent { get; set; } = "";


        public OutputController()
        {
            Action = new List<string>();
            Hit = new List<string>();
            Status = new List<string>();
            Hint = new List<string>();
            Special = new List<string>();
            Info = new List<string>();
            Error = new List<string>();
            Damage = 0;
        }

        public enum MessageType
        {
            Action = 0,
            Damage = 1,
            Hit = 2,
            Hint = 3,
            Special = 4
        }

        public string FormatMessage(MessageType messageType, string message)
        {
            switch (messageType)
            {
                case MessageType.Action:
                    return "Action: " + message + " ";
                case MessageType.Damage:
                    return "[color=yellow]( Damage: " + message + " )[/color]";
                case MessageType.Hit:
                    return "[color=red][b]" + message + "[/b][/color]";
                case MessageType.Hint:
                    return "[color=cyan]" + message + "[/color]";
                case MessageType.Special:
                    return "\n[color=red]" + message + "[/color]";
                default:
                    return "";
            }
        }

        public void Broadcast(Battlefield battlefield)
        {
            Info.Add("This is " + battlefield.GetActor().Name + "'s turn.");
            var lines = new List<string>(); ;
            lines.Add("");
            if (Action.Count > 0) lines[0] += FormatMessage(MessageType.Action, string.Join(" ", Action));
            if (Damage != 0) lines[0] += FormatMessage(MessageType.Damage, Damage.ToString());
            if (lines[0] == "") lines.Clear();

            if (Hit.Count > 0) lines.Add(FormatMessage(MessageType.Hit, string.Join("\n", Hit)));
            if (Status.Count > 0) lines.Add(string.Join("\n", Status));
            if (Hint.Count > 0) lines.Add(FormatMessage(MessageType.Hint, string.Join("\n", Hint)));
            if (Special.Count > 0) lines.Add(FormatMessage(MessageType.Special, string.Join("\n", Special)));
            if (Info.Count > 0) lines.Add("\n" + string.Join("\n", Info));

            LastMessageSent = string.Join("\n", lines);

            battlefield.Plugin.FChatClient.SendMessageInChannel(LastMessageSent, battlefield.Plugin.Channel);
            if (Error.Count > 0)
            {
                battlefield.Plugin.FChatClient.SendMessageInChannel(string.Join("\n", Error), battlefield.Plugin.Channel);
            }

            //clear messages from the queue once they have been displayed
            Action.Clear();
            Hit.Clear();
            Damage = 0;
            Status.Clear();
            Hint.Clear();
            Info.Clear();
            Error.Clear();
        }
    }
}
