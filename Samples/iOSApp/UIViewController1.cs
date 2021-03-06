using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using InTheHand.UI.Popups;
using System.Threading.Tasks;
using InTheHand.ApplicationModel;
using InTheHand.Storage;
using InTheHand.Foundation.Collections;
using InTheHand.Devices.Sensors;
using System.IO;
using InTheHand.Devices.Bluetooth.GenericAttributeProfile;

namespace ApplicationModel.iOS
{
    [Register("UniversalView")]
    public class UniversalView : UIView
    {
        public UniversalView()
        {
            Initialize();
        }

        public UniversalView(RectangleF bounds) : base(bounds)
        {
            Initialize();
        }

        void Initialize()
        {
            BackgroundColor = UIColor.FromRGB(0x0, 0x66, 0x66);

            Task.Run(async () =>
            {
                await Task.Delay(3000);
                BeginInvokeOnMainThread(async () =>
                {
                    
                    /*
                    string q = InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.GetDeviceSelectorFromUuid(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.Battery);
                    var devs = await InTheHand.Devices.Enumeration.DeviceInformation.FindAllAsync(q);
                    MessageDialog md = new MessageDialog("test", "Title");
                    UICommand oneC = new UICommand("One", null);
                    UICommand twoC = new UICommand("Two", null);
                    md.Commands.Add(oneC);
                    md.Commands.Add(twoC);
                    IUICommand chosen = await md.ShowAsync();

                    bool success = chosen == oneC;*/
                });
            });
        }

        private void Values_MapChanged(IObservableMap<string, object> sender, IMapChangedEventArgs<string> eventArgs)
        {
            System.Diagnostics.Debug.WriteLine(eventArgs.CollectionChange.ToString() + " " + eventArgs.Key);
        }
    }

    [Register("UIViewController1")]
    public class UIViewController1 : UIViewController
    {
        public UIViewController1()
        {
            //InTheHand.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView().DataRequested += UIViewController1_DataRequested;
        }

        /*private void UIViewController1_DataRequested(object sender, InTheHand.ApplicationModel.DataTransfer.DataRequestedEventArgs e)
        {
            e.Request.Data.SetText("Hello World!");
            e.Request.Data.Properties.Title = "New Share";
        }*/

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        private async void B_TouchUpInside(object sender, EventArgs e)
        {
            var prof = InTheHand.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile();

            //UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("App-Prefs:root=BLUETOOTH"));
            //await InTheHand.System.Launcher.LaunchUriAsync(new Uri("App-Prefs:root=AIRPLANE_MODE"));
           
            string aqsFilter = InTheHand.Devices.Bluetooth.BluetoothLEDevice.GetDeviceSelector();
            var devices = await InTheHand.Devices.Enumeration.DeviceInformation.FindAllAsync(aqsFilter);
            foreach(InTheHand.Devices.Enumeration.DeviceInformation di in devices)
            {
                System.Diagnostics.Debug.WriteLine(di);
                var btd = await InTheHand.Devices.Bluetooth.BluetoothLEDevice.FromIdAsync(di.Id);
                foreach(GattDeviceService serv in btd.GattServices )
                {
                    foreach(GattCharacteristic cha in serv.GetAllCharacteristics())
                    {
                        System.Diagnostics.Debug.WriteLine(cha.UserDescription);
                    }
                }
            }
            //var file = await InTheHand.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("newfile.html", CreationCollisionOption.ReplaceExisting);
            //await FileIO.WriteTextAsync(file, "one,two,three,four,five");
            //var success = await InTheHand.System.Launcher.LaunchFileAsync(file);
            /*InTheHand.ApplicationModel.Email.EmailMessage m = new InTheHand.ApplicationModel.Email.EmailMessage();
            m.To.Add(new InTheHand.ApplicationModel.Email.EmailRecipient("peter@peterfoot.net"));
            m.Subject = "Here's your file";
            m.Body = "File attached!!!";
            m.Attachments.Add(new InTheHand.ApplicationModel.Email.EmailAttachment("test.txt", file));
            await InTheHand.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(m);*/

            /*InTheHand.Media.Capture.CameraCaptureUI ccu = new InTheHand.Media.Capture.CameraCaptureUI();
            StorageFile sf = await ccu.CaptureFileAsync(InTheHand.Media.Capture.CameraCaptureUIMode.Photo);
            var p = await sf.GetBasicPropertiesAsync();*/
        }

        private Accelerometer a;

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            a = Accelerometer.GetDefault();
            //a.ReadingChanged += A_ReadingChanged;
            //var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("test.txt", CreationCollisionOption.OpenIfExists);
            //System.Diagnostics.Debug.WriteLine(file.ContentType);

            /* Windows.UI.Popups.PopupMenu pm = new Windows.UI.Popups.PopupMenu();
             pm.Commands.Add(new UICommand("First", (c) => { }));
             pm.Commands.Add(new UICommand("Second", (c) => { }));
             pm.Commands.Add(new UICommand("Third", (c) => { }));
             pm.Commands.Add(new UICommand("Fourth", (c) => { }));
             pm.Commands.Add(new UICommand("Fifth", (c) => { }));
             pm.Commands.Add(new UICommand("Sixth", (c) => { }));
             await pm.ShowAsync(new Windows.Foundation.Point() { X = 20, Y = 20 });*/



            DateTimeOffset? isThis = DateTimeOffset.Now;
            ApplicationData.Current.LocalSettings.Values["LastUpload"] = isThis;

            /*InTheHand.UI.Popups.MessageDialog md = new InTheHand.UI.Popups.MessageDialog("Content", "Title");
            md.Commands.Add(new UICommand("One", (c) => { System.Diagnostics.Debug.WriteLine("One"); }));
            md.Commands.Add(new UICommand("Two", (c) => { System.Diagnostics.Debug.WriteLine("Two"); }));
            //md.Commands.Add(new UICommand("Three", (c) => { System.Diagnostics.Debug.WriteLine("Three"); }));
            await md.ShowAsync();*/

            System.Diagnostics.Debug.WriteLine(Package.Current.DisplayName);
            System.Diagnostics.Debug.WriteLine(Package.Current.Id.Name);
            System.Diagnostics.Debug.WriteLine(Package.Current.Id.Version.ToString());
            System.Diagnostics.Debug.WriteLine(Package.Current.InstalledDate);
            System.Diagnostics.Debug.WriteLine(Package.Current.IsDevelopmentMode);

        }

        private void A_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
            {
                t.Text = args.Reading.AccelerationX + " " + args.Reading.AccelerationY + " " + args.Reading.AccelerationZ + " " + args.Reading.Timestamp + " " + DateTimeOffset.Now;
            });
            System.Diagnostics.Debug.WriteLine(args.Reading.AccelerationX + " " + args.Reading.AccelerationY + " " + args.Reading.AccelerationZ + " " + args.Reading.Timestamp + " " + DateTimeOffset.Now);
        }

        internal UILabel t;
        public override void ViewDidLoad()
        {
            View = new UniversalView();
            var b = new UIButton(UIButtonType.System);
            b.Center = new CoreGraphics.CGPoint(200, 200);
            b.Bounds = new CoreGraphics.CGRect(0, 0, 400, 200);
            b.SetTitle("Camera", UIControlState.Normal);
            b.TouchUpInside += B_TouchUpInside;
            View.Add(b);

            t = new UILabel();
            t.Center = new CoreGraphics.CGPoint(200, 400);
            t.Bounds = new CoreGraphics.CGRect(0, 0, 400, 400);
            View.Add(t);

            base.ViewDidLoad();

            
            // Perform any additional setup after loading the view
        }
    }
}