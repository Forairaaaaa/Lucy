using System.Drawing;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Lucy.Services;
using Lucy.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;

namespace Lucy.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();

        //// Set title bar 
        //App.MainWindow.SetTitleBar(MainPageTitleBar);






        
        Paragraph paragraph = new Paragraph();
        Run run = new Run();

        // Customize some properties on the RichTextBlock.
        SerialReceiveTextBlock.IsTextSelectionEnabled = true;
        run.Text = "I (851) spi_flash: flash io: qio\r\nI (856) sleep: Configure to isolate all GPIO pins in sleep state\r\nI (862) sleep: Enable automatic switching of GPIO sleep configuration\r\nI (869) app_start: Starting scheduler on CPU0\r\nI (874) app_start: Starting scheduler on CPU1\r\nI (874) main_task: Started on CPU0\r\nI (884) esp_psram: Reserving pool of 32K of internal memory for DMA/internal allocations\r\nI (892) main_task: Calling app_main()\r\nI (897) hal: init hal\r\nI (899) hal: start holding power\r\nI (903) gpio: GPIO[46]| InputEn: 0| OutputEn: 0| OpenDrain: 0| Pullup: 1| Pulldown: 0| Intr:0 \r\nI (913) hal: init display\r\nI (1190) hal: create framebuffer\r\nI (1194) gpio: GPIO[21]| InputEn: 0| OutputEn: 0| OpenDrain: 0| Pullup: 1| Pulldown: 0| Intr:0 \r\nI (1194) gpio: GPIO[0]| InputEn: 0| OutputEn: 0| OpenDrain: 0| Pullup: 1| Pulldown: 0| Intr:0 \r\n[1970-01-01 00:00:14.526] [info] mooncake init :)\r\n[1970-01-01 00:00:14.530] [info] create userdata\r\n[1970-01-01 00:00:14.534] [info] start db setup\r\n[1970-01-01 00:00:14.538] [info] create boot anim\r\n[1970-01-01 00:00:14.543] [info] start boot anim\r\n\r\n _____ _____ _____ _____ _____ _____ _____ _____\r\n|     |     |     |   | |     |  _  |  |  |   __|\r\n| | | |  |  |  |  | | | |   --|     |    -|   __|\r\n|_|_|_|_____|_____|_|___|_____|__|__|__|__|_____|\r\n\r\n- @author Forairaaaaa\r\n- @version v0.2.0\r\n- @build at 07:44:44 Aug 25 2023\r\n\r\n[1970-01-01 00:00:14.572] [info] init done\r\n";


        // Add the Run to the Paragraph, the Paragraph to the RichTextBlock.
        paragraph.Inlines.Add(run);


        //paragraph.Foreground = new SolidColorBrush(Colors.GreenYellow);

        SerialReceiveTextBlock.Blocks.Add(paragraph);





        paragraph = new Paragraph();
        run = new Run();
        // Customize some properties on the RichTextBlock.
        SerialReceiveTextBlock.IsTextSelectionEnabled = true;
        run.Text = "I (851) spi_flash: flash io: qio\r\nI (856) sleep: Configure to isolate all GPIO pins in sleep state\r\nI (862) sleep: Enable automatic switching of GPIO sleep configuration\r\nI (869) app_start: Starting scheduler on CPU0\r\nI (874) app_start: Starting scheduler on CPU1\r\nI (874) main_task: Started on CPU0\r\nI (884) esp_psram: Reserving pool of 32K of internal memory for DMA/internal allocations\r\nI (892) main_task: Calling app_main()\r\nI (897) hal: init hal\r\nI (899) hal: start holding power\r\nI (903) gpio: GPIO[46]| InputEn: 0| OutputEn: 0| OpenDrain: 0| Pullup: 1| Pulldown: 0| Intr:0 \r\nI (913) hal: init display\r\nI (1190) hal: create framebuffer\r\nI (1194) gpio: GPIO[21]| InputEn: 0| OutputEn: 0| OpenDrain: 0| Pullup: 1| Pulldown: 0| Intr:0 \r\nI (1194) gpio: GPIO[0]| InputEn: 0| OutputEn: 0| OpenDrain: 0| Pullup: 1| Pulldown: 0| Intr:0 \r\n[1970-01-01 00:00:14.526] [info] mooncake init :)\r\n[1970-01-01 00:00:14.530] [info] create userdata\r\n[1970-01-01 00:00:14.534] [info] start db setup\r\n[1970-01-01 00:00:14.538] [info] create boot anim\r\n[1970-01-01 00:00:14.543] [info] start boot anim\r\n\r\n _____ _____ _____ _____ _____ _____ _____ _____\r\n|     |     |     |   | |     |  _  |  |  |   __|\r\n| | | |  |  |  |  | | | |   --|     |    -|   __|\r\n|_|_|_|_____|_____|_|___|_____|__|__|__|__|_____|\r\n\r\n- @author Forairaaaaa\r\n- @version v0.2.0\r\n- @build at 07:44:44 Aug 25 2023\r\n\r\n[1970-01-01 00:00:14.572] [info] init done\r\n";
        // Add the Run to the Paragraph, the Paragraph to the RichTextBlock.
        paragraph.Inlines.Add(run);
        //paragraph.Foreground = new SolidColorBrush(Colors.OrangeRed);
        SerialReceiveTextBlock.Blocks.Add(paragraph);

    }
}
