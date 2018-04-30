using System;

namespace PreflightHQ.ActionExecutor
{
    public class ClientAction
    {
        public string Type { get; set; }
        public ActionType ActionType
        {
            get
            {
                switch (Type)
                {
                    case "click":
                        return ActionType.Click;
                    case "type":
                        return ActionType.Type;
                    case "navigate":
                        return ActionType.Navigate;
                    case "set-checkpoint":
                        return ActionType.SetCheckpoint;
                    case "enter":
                        return ActionType.Enter;
                    case "select":
                        return ActionType.Select;
                }
                throw new Exception("Could not determine the action.");
            }
        }

        public string Value { get; set; }

        public string Selector { get; set; }
    }
}
