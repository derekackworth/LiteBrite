/*	
	Author: Derek Ackworth
	Date: April 5th, 2019
	File: MainViewModel.cs
    Purpose: MainViewModel class implementation
*/

using LiteBrite.Helpers;
using LiteBrite.Models;
using LiteBrite.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LiteBrite.ViewModels
{
    class MainViewModel
    {
        // RelayCommands
        public RelayCommand OpenCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand NewCommand { get; set; }
        public RelayCommand RandomCommand { get; set; }
        public RelayCommand AboutCommand { get; set; }

        // LiteBriteModel
        public LiteBriteModel LiteBrite { get; set; }

        // Default Constructor
        public MainViewModel()
        {
            // Initialize the RelayCommands
            OpenCommand = new RelayCommand(OpenBoard);
            SaveCommand = new RelayCommand(SaveBoard);
            ExitCommand = new RelayCommand(ExitLiteBrite);
            NewCommand = new RelayCommand(NewBoard);
            RandomCommand = new RelayCommand(RandomBoard);
            AboutCommand = new RelayCommand(AboutLiteBrite);

            // Initialize the LiteBriteModel
            LiteBrite = new LiteBriteModel
            {
                // Initialize the Menu
                Menu = new ObservableCollection<MenuItem>
                {
                    new MenuItem
                    {
                        Header = Resources.file,
                        ItemsSource = new ObservableCollection<FrameworkElement>
                        {
                            new MenuItem
                            {
                                Header = Resources.open,
                                InputGestureText = "Ctrl+O",
                                Command = OpenCommand
                            },
                            new MenuItem
                            {
                                Header = Resources.save,
                                InputGestureText = "Ctrl+S",
                                Command = SaveCommand
                            },
                            new Separator(),
                            new MenuItem
                            {
                                Header = Resources.exit,
                                InputGestureText = "Alt+F4",
                                Command = ExitCommand
                            }
                        }
                    },
                    new MenuItem
                    {
                        Header = Resources.game,
                        ItemsSource = new ObservableCollection<FrameworkElement>
                        {
                            new MenuItem
                            {
                                Header = Resources.newG,
                                InputGestureText = "Ctrl+N",
                                Command = NewCommand
                            },
                            new MenuItem
                            {
                                Header = Resources.random,
                                InputGestureText = "Ctrl+R",
                                Command = RandomCommand
                            }
                        }
                    },
                    new MenuItem
                    {
                        Header = Resources.help,
                        ItemsSource = new ObservableCollection<FrameworkElement>
                        {
                            new MenuItem
                            {
                                Header = Resources.about,
                                InputGestureText = "Ctrl+H",
                                Command = AboutCommand
                            }
                        }
                    }
                },
                // Initialize the Palette
                Palette = new ObservableCollection<Ellipse>
                {
                    new Ellipse
                    {
                        Fill = Brushes.Black,
                        Width = 10,
                        Height = 10
                    },
                    new Ellipse
                    {
                        Fill = Brushes.White,
                        Width = 10,
                        Height = 10
                    },
                    new Ellipse
                    {
                        Fill = Brushes.Red,
                        Width = 10,
                        Height = 10
                    },
                    new Ellipse
                    {
                        Fill = Brushes.Green,
                        Width = 10,
                        Height = 10
                    },
                    new Ellipse
                    {
                        Fill = Brushes.Blue,
                        Width = 10,
                        Height = 10
                    },
                    new Ellipse
                    {
                        Fill = Brushes.Yellow,
                        Width = 10,
                        Height = 10
                    },
                    new Ellipse
                    {
                        Fill = Brushes.Magenta,
                        Width = 10,
                        Height = 10
                    },
                    new Ellipse
                    {
                        Fill = Brushes.Cyan,
                        Width = 10,
                        Height = 10
                    },
                    new Ellipse
                    {
                        Fill = Brushes.Purple,
                        Width = 10,
                        Height = 10
                    },
                    new Ellipse
                    {
                        Fill = Brushes.Maroon,
                        Width = 10,
                        Height = 10
                    },
                    new Ellipse
                    {
                        Fill = Brushes.Orange,
                        Width = 10,
                        Height = 10
                    },
                    new Ellipse
                    {
                        Fill = Brushes.Lime,
                        Width = 10,
                        Height = 10
                    },
                    new Ellipse
                    {
                        Fill = Brushes.Teal,
                        Width = 10,
                        Height = 10
                    },
                    new Ellipse
                    {
                        Fill = Brushes.Navy,
                        Width = 10,
                        Height = 10
                    },
                    new Ellipse
                    {
                        Fill = Brushes.Brown,
                        Width = 10,
                        Height = 10
                    },
                    new Ellipse
                    {
                        Fill = Brushes.Olive,
                        Width = 10,
                        Height = 10
                    }
                }
            };

            // Initialize the Board
            for (int i = 0; i < 50; i++)
            {
                LiteBrite.Board.Add(new ObservableCollection<Ellipse>());

                for (int j = 0; j < 50; j++)
                {
                    LiteBrite.Board[i].Add(new Ellipse
                    {
                        Fill = Brushes.Black,
                        Width = 10,
                        Height = 10
                    });
                }
            }
        }

        // Open a Board from a file
        private void OpenBoard(object obj)
        {
            string _fileDirectory = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(Directory.GetCurrentDirectory(), ".\\"));

            OpenFileDialog dig = new OpenFileDialog
            {
                InitialDirectory = _fileDirectory,
                Filter = Resources.binFilter
            };

            if (dig.ShowDialog() == true)
            {
                FileStream fs = new FileStream(dig.FileName, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();

                try
                {
                    Tuple<List<List<string>>, List<List<double>>, List<List<double>>> tuple =
                        (Tuple<List<List<string>>, List<List<double>>, List<List<double>>>)formatter.Deserialize(fs);

                    for (int i = 0; i < tuple.Item1.Count; i++)
                    {
                        for (int j = 0; j < tuple.Item1[i].Count; j++)
                        {
                            LiteBrite.Board[i][j] = new Ellipse
                            {
                                Fill = (Brush)new BrushConverter().ConvertFromString(tuple.Item1[i][j]),
                                Width = 10,
                                Height = 10
                            };
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        // Save the Board to a file
        private void SaveBoard(object obj)
        {
            string _fileDirectory = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(Directory.GetCurrentDirectory(), ".\\"));

            SaveFileDialog dig = new SaveFileDialog
            {
                InitialDirectory = _fileDirectory,
                Filter = Resources.binFilter
            };

            if (dig.ShowDialog() == true)
            {
                FileStream fs = new FileStream(dig.FileName, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();

                try
                {
                    List<List<string>> boardFills = new List<List<string>>();
                    List<List<double>> boardWidths = new List<List<double>>();
                    List<List<double>> boardHeights = new List<List<double>>();

                    for (int i = 0; i < LiteBrite.Board.Count; i++)
                    {
                        boardFills.Add(new List<string>());
                        boardWidths.Add(new List<double>());
                        boardHeights.Add(new List<double>());

                        for (int j = 0; j < LiteBrite.Board[i].Count; j++)
                        {
                            boardFills[i].Add(LiteBrite.Board[i][j].Fill.ToString());
                            boardWidths[i].Add(LiteBrite.Board[i][j].Width);
                            boardHeights[i].Add(LiteBrite.Board[i][j].Height);
                        }
                    }

                    formatter.Serialize(fs, Tuple.Create(boardFills, boardWidths, boardHeights));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        // Exit the LiteBrite
        private void ExitLiteBrite(object obj)
        {
            Application.Current.MainWindow.Close();
        }

        // Clear the Board
        private void NewBoard(object obj)
        {
            for (int i = 0; i < LiteBrite.Board.Count; i++)
            {
                for (int j = 0; j < LiteBrite.Board[i].Count; j++)
                {
                    LiteBrite.Board[i][j] = new Ellipse
                    {
                        Fill = Brushes.Black,
                        Width = 10,
                        Height = 10
                    };
                }
            }
        }

        // Randomize the Board
        private void RandomBoard(object obj)
        {
            Random random = new Random();

            for (int i = 0; i < LiteBrite.Board.Count; i++)
            {
                for (int j = 0; j < LiteBrite.Board[i].Count; j++)
                {
                    Ellipse ellipse = LiteBrite.Palette[random.Next(LiteBrite.Palette.Count)];

                    LiteBrite.Board[i][j] = new Ellipse
                    {
                        Fill = ellipse.Fill,
                        Width = ellipse.Width,
                        Height = ellipse.Height
                    };
                }
            }
        }

        // Show a MessageBox that has the program name and the author name
        private void AboutLiteBrite(object obj)
        {
            MessageBox.Show($"{Resources.program}{Resources.title}\n" +
                $"{Resources.author}{Resources.authorName}");
        }
    }
}
