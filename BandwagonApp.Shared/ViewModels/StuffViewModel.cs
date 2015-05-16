using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Band;
using Microsoft.Band.Notifications;
using Microsoft.Band.Tiles;
using Microsoft.Band.Tiles.Pages;
using SuiteValue.UI.MVVM;

namespace BandwagonApp.ViewModels
{
    public class StuffViewModel : ViewModelBase
    {
        private readonly Guid _tileGuid = new Guid("5DC94C4B-C407-46E0-B9E2-BBD2FFC69E52");
        private readonly IBandClient _client;

        public StuffViewModel(IBandClient bandClient)
        {
            _client = bandClient;
        }

        private async Task<BandIcon> LoadIcon(string uri)
        {
            StorageFile imageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(uri));

            using (IRandomAccessStream fileStream = await imageFile.OpenAsync(FileAccessMode.Read))
            {
                WriteableBitmap bitmap = new WriteableBitmap(1, 1);
                await bitmap.SetSourceAsync(fileStream);
                return bitmap.ToBandIcon();
            }
        }

        private DelegateCommand _createTileCommand;

        public DelegateCommand CreateTileCommand
        {
            get
            {
                return _createTileCommand ?? (_createTileCommand = new DelegateCommand(
                    async () =>
                    {
                        // create a new tile with a new TileGuid
                        BandTile tile = new BandTile(_tileGuid)
                        {
                            // enable badging (the count of unread messages)
                            IsBadgingEnabled = true,
                            // set the name
                            Name = "Bandwagon",
                            // set the icons
                            SmallIcon = await LoadIcon("ms-appx:///Assets/baby_icon2_24x24.png"),
                            TileIcon = await LoadIcon("ms-appx:///Assets/baby_icon2_46x46.png"),
                        };

                        try
                        {
                            // get the current set of tiles
                            IEnumerable<BandTile> tiles = await
                                _client.TileManager.GetTilesAsync();
                            foreach (var t in tiles)
                            {
                                // remove the tile from the Band
                                if (await _client.TileManager.RemoveTileAsync(t))
                                {
                                    // do work if the tile was successfully removed
                                }
                            }
                        }
                        catch (BandException ex)
                        {
                            // handle a Band connection exception
                        }

                        try
                        {
                            var result = await _client.TileManager.GetRemainingTileCapacityAsync();
                            if (result > 0)
                            {
                                await _client.TileManager.AddTileAsync(tile);
                            }
                        }
                        catch (BandException ex)
                        {
                        }

                    }));
            }
        }

        private DelegateCommand _addPageCommand;

        public DelegateCommand AddPageCommand
        {
            get
            {
                return _addPageCommand ?? (_addPageCommand = new DelegateCommand(async () =>
                    {
                        ScrollFlowPanel panel = new ScrollFlowPanel
                        {
                            Rect = new PageRect(0, 0, 245, 102),
                            Orientation = FlowPanelOrientation.Horizontal,
                        };
                        panel.Elements.Add(
                            new WrappedTextBlock
                            {
                                ElementId = 1,
                                Rect = new PageRect(0, 0, 245, 102),
                                // left, top, right, bottom margins
                                Margins = new Margins(15, 0, 15, 0),
                                Color = new BandColor(0xFF, 0xFF, 0xFF),
                                Font = WrappedTextBlockFont.Small
                            });
                        PageLayout layout = new PageLayout(panel);
                        var tiles = await _client.TileManager.GetTilesAsync();
                        var tile = tiles.FirstOrDefault();
                        if (tile != null)
                        {
                            tile.PageLayouts.Add(layout);
                        }



                    }));
            }
        }

        private DelegateCommand _setPageCommand;

        public DelegateCommand SetPageCommand
        {
            get
            {
                return _setPageCommand ?? (_setPageCommand = new DelegateCommand(
                    async () =>
                    {
                        // create a new Guid for the messages page
                        Guid messagesPageGuid = Guid.NewGuid();
                        // create the object containing the page content to be set
                        PageData pageContent = new PageData(
                            messagesPageGuid,
                            // Specify which layout to use for this page
                            0,
                            new WrappedTextBlockData(
                                1,
                                "This is the text of the first message"));
                        try
                        {
                            // set the page content on the Band
                            if (await _client.TileManager.SetPagesAsync(_tileGuid,
                           pageContent))
                            {
                                // page content successfully set on Band
                            }
                            else
                            {
                                // unable to set content to the Band
                            }
                        }
                        catch (BandException ex)
                        {
                            // handle a Band connection exception
                        }
                    }));
            }
        }










        private DelegateCommand _openDialogCommand;

        public DelegateCommand OpenDialogCommand
        {
            get
            {
                return _openDialogCommand ?? (_openDialogCommand = new DelegateCommand(
                    async () =>
                    {
                        try
                        {
                            // send a message to the Band for one of our tiles, and show it as a dialog as well
                             await _client.NotificationManager.SendMessageAsync(_tileGuid,
                                "Jump on the wagon?", "Up to you to decide", DateTimeOffset.Now,
                                MessageFlags.ShowDialog);
                        }
                        catch (BandException ex)
                        {
                            // handle a Band connection exception
                        }

                    }));
            }
        }

        private DelegateCommand _sendVibrationCommand;

        public DelegateCommand SendVibrationCommand
        {
            get
            {
                return _sendVibrationCommand ?? (_sendVibrationCommand = new DelegateCommand(
                   async () =>
                    {
                        try
                        {
                            // send a vibration request of type alert alarm to the Band
                            await
                                _client.NotificationManager.VibrateAsync(VibrationType.NotificationAlarm);
                            var bandimage = await _client.PersonalizationManager.GetMeTileImageAsync();
                        }
                        catch (BandException ex)
                        {
                            // handle a Band connection exception
                        }
                    }));
            }
        }




    }
}