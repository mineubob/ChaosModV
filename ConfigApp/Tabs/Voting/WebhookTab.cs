using System.Windows;
using System.Windows.Controls;

namespace ConfigApp.Tabs.Voting
{
    public class WebhookTab : Tab
    {
        private CheckBox? m_EnableWebhookVoting = null;

        private TextBox? m_Address = null;
        private TextBox? m_Port = null;

        private void SetElementsEnabled(bool state)
        {
            if (m_Address is not null)
            {
                m_Address.IsEnabled = state;
            }
            if (m_Port is not null)
            {
                m_Port.IsEnabled = state;
            }
        }

        protected override void InitContent()
        {
            PushNewColumn(new GridLength(340f));
            PushNewColumn(new GridLength(10f));
            PushNewColumn(new GridLength(150f));
            PushNewColumn(new GridLength(250f));
            PushNewColumn(new GridLength(10f));
            PushNewColumn(new GridLength());

            PushRowEmpty();
            PushRowEmpty();
            PushRowExpandElement(new TextBlock()
            {
                Text = "NOTE: This is still an experimental feature!",
                HorizontalAlignment = HorizontalAlignment.Left
            });
            PopRow();

            PushRowEmpty();
            PushRowEmpty();
            PushRowEmpty();
            m_EnableWebhookVoting = new CheckBox()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Content = "Enable Webhook Voting"
            };
            m_EnableWebhookVoting.Click += (sender, eventArgs) =>
            {
                SetElementsEnabled(m_EnableWebhookVoting.IsChecked.GetValueOrDefault());
            };
            PushRowElement(m_EnableWebhookVoting);
            PopRow();

            PushRowEmpty();
            PushRowSpacedPair("Webhook Address", m_Address = new TextBox()
            {
                Width = 125f,
                Height = 20f
            });
            PushRowSpacedPair("Webhook Port", m_Port = new TextBox()
            {
                Width = 125f,
                Height = 20f
            });

            SetElementsEnabled(false);
        }

        public override void OnLoadValues()
        {
            if (m_EnableDiscordVoting is not null)
            {
                m_EnableDiscordVoting.IsChecked = OptionsManager.TwitchFile.ReadValueBool("EnableVotingWebhook", false);
                SetElementsEnabled(m_EnableDiscordVoting.IsChecked.GetValueOrDefault());
            }

            if (m_Address is not null)
            {
                m_Address.Text = OptionsManager.TwitchFile.ReadValue("WebhookAddress");
            }
            if (m_Port is not null)
            {
                m_Port.Text = OptionsManager.TwitchFile.ReadValue("WebhookPort");
            }
        }

        public override void OnSaveValues()
        {
            OptionsManager.TwitchFile.WriteValue("EnableVotingWebhook", m_EnableDiscordVoting?.IsChecked);

            OptionsManager.TwitchFile.WriteValue("WebhookAddress", m_Address?.Text);
            OptionsManager.TwitchFile.WriteValue("WebhookPort", m_Port?.Text);
        }
    }
}
