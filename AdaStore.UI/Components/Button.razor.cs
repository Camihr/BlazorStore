using AdaStore.UI.UI;
using Microsoft.AspNetCore.Components;

namespace AdaStore.UI.Components
{
    public partial class Button
    {
        [Parameter] public EventCallback Click { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public bool IsFull { get; set; }
        [Parameter] public Buttons Style { get; set; }
        [Parameter] public bool IsSubmit { get; set; }
       
        private string _btnClass;

        protected override void OnInitialized()
        {
            _btnClass = "btn btn-";

            switch (Style)
            {
                case Buttons.Primary:
                    _btnClass += "primary";
                    break;
                case Buttons.Secondary:
                    _btnClass += "secondary";
                    break;
                case Buttons.Default:
                    _btnClass += "default";
                    break;
                case Buttons.Danger:
                    _btnClass += "danger";
                    break;
            }

            if (IsFull)
            {
                _btnClass += " btn-full";
            }
        }
    }
}
