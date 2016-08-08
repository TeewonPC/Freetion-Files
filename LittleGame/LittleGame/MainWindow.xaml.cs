using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LittleGame
{
    /// <summary>
    /// 小游戏处理
    /// </summary>
    public partial class MainWindow : Window
    {
        // 各种动物的速度*10
        private int iSquabSpeed = 5;
        private int iCrabSpeed = 1;
        private int iSnakeSpeed = 2;
        private int iPigSpeed = 3;

        // 游戏开始秒数 /10
        private int iSec = 0;

        // 游戏参数
        private int HP = 10000;         // 题主生命值
        private int animalNum = 0;      // 游戏总动物数量
        private int animalHunt = 0;     // 抓捕的动物数量
        private int animalEscape = 0;   // 逃跑的动物数量

        // 逃跑后给题主造成的伤害
        private int iSquabHurts = 100;
        private int iCrabHurts = 200;
        private int iSnakeHurts = 500;
        private int iPigHurts = 1000;

        // 正常退出游戏标志
        private bool iFinish = true;

        System.Timers.Timer tmp = new System.Timers.Timer(100);//实例化Timer类，设置时间间隔
        public MainWindow()
        {
            InitializeComponent();
           
            tmp.Elapsed += new System.Timers.ElapsedEventHandler(AnimalMove);//到达时间的时候执行事件
            tmp.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
            tmp.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件

            this.Closed += MainWindow_Closed;

        }
        /// <summary>
        /// 退出游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (iFinish)
            {
                if (HP >= 9000)
                {
                    MessageBox.Show("游戏结束！\n\n恭喜闯关成功：获得三颗星！！！");
                }
                else if (HP >= 6000)
                {
                    MessageBox.Show("游戏结束！\n\n恭喜闯关成功：获得两颗星！！");
                }
                else if (HP > 0)
                {
                    MessageBox.Show("游戏结束！\n\n恭喜闯关成功：获得一颗星！");
                }
                else
                {
                    MessageBox.Show("游戏结束！\n\n闯关失败：题主已去地狱忏悔了（题主女儿痛苦流涕）。。。");
                }
            }
            tmp.Stop();
            tmp.Close();
        }

        void AnimalMove(object source, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => 
            {
                if (animalNum < 50)
                {
                    if (iSec % 30 == 0)
                    {
                        Brush bru = new SolidColorBrush(Colors.Green);
                        int rand = getRand();
                        AddAnimal(bru, rand);
                    }
                    if (iSec % 40 == 0)
                    {
                        Brush bru = new SolidColorBrush(Colors.Black);
                        int rand = getRand();
                        AddAnimal(bru, rand);
                    }
                    if (iSec % 50 == 0)
                    {
                        Brush bru = new SolidColorBrush(Colors.Blue);
                        int rand = getRand();
                        AddAnimal(bru, rand);
                    }
                    if (iSec % 80 == 0)
                    {
                        Brush bru = new SolidColorBrush(Colors.Red);
                        int rand = getRand();
                        AddAnimal(bru, rand);
                    }
                    ++iSec;
                    iSec %= 480;
                }

                if (animalNum == 50 && (animalEscape + animalHunt == animalNum)) 
                {
                    this.Close();
                }
                if (HP <= 0)
                {
                    this.Close();
                }
                foreach(Ellipse ell in this.PutAnimal.Children)
                {
                    Double xPos = Canvas.GetLeft(ell);
                    Double yPos = Canvas.GetTop(ell);

                    Double Wid = this.PutAnimal.ActualWidth;
                    Double Hei = this.PutAnimal.ActualHeight;

                    // Pig
                    if (ell.Fill.ToString() == Colors.Red.ToString())
                    {
                        if (ell.Visibility != Visibility.Collapsed && (xPos > Wid || yPos > Hei))
                        {
                            ell.Visibility = Visibility.Collapsed;
                            HP -= iPigHurts;
                            ++animalEscape;
                        }
                        
                        Canvas.SetTop(ell, yPos + iPigSpeed);
                    }

                    // Snake
                    else if (ell.Fill.ToString() == Colors.Blue.ToString())
                    {
                        if (ell.Visibility != Visibility.Collapsed && (xPos > Wid || yPos > Hei))
                        {
                            ell.Visibility = Visibility.Collapsed;
                            HP -= iSnakeHurts;
                            ++animalEscape;
                        }

                        Canvas.SetLeft(ell, xPos + 5 * Math.Sin(yPos));
                        Canvas.SetTop(ell, yPos + iSnakeSpeed);
                    }

                    // Crab
                    else if (ell.Fill.ToString() == Colors.Black.ToString())
                    {
                        if (ell.Visibility != Visibility.Collapsed && (xPos > Wid || yPos > Hei))
                        {
                            ell.Visibility = Visibility.Collapsed;
                            HP -= iCrabHurts;
                            ++animalEscape;
                        }

                        Canvas.SetLeft(ell, xPos + iCrabSpeed);
                    }

                    // Squab
                    else
                    {
                        if (ell.Visibility != Visibility.Collapsed && (xPos > Wid || yPos > Hei))
                        {
                            ell.Visibility = Visibility.Collapsed; 
                            HP -= iSquabHurts;
                            ++animalEscape;
                        }

                        Canvas.SetLeft(ell, xPos + iSquabSpeed * Math.Pow((Wid - xPos), 2)
                            / (Math.Pow((Wid - xPos), 2) + Math.Pow((Hei - yPos),2)));
                        Canvas.SetTop(ell, yPos + iSquabSpeed * Math.Pow((Hei - yPos), 2)
                            / (Math.Pow((Wid - xPos), 2) + Math.Pow((Hei - yPos), 2)));
                    }
                }
            }));
        }

        /// <summary>
        /// 增加动物
        /// </summary>
        /// <param name="bru">代表颜色</param>
        /// <param name="xPos">初始位置</param>
        private void AddAnimal(Brush bru, int xPos)
        {
            Ellipse animal = new Ellipse();
            animal.Width = 20;
            animal.Height = 20;
            animal.Fill = bru;
            animal.MouseDown += ellipse_MouseLeftButtonDown;

            Canvas.SetLeft(animal, xPos);
            Canvas.SetTop(animal, 0);
            this.PutAnimal.Children.Add(animal);

            ++animalNum;
        }

        /// <summary>
        /// 抓到动物
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var ell = sender as Ellipse;
            ell.Visibility = Visibility.Collapsed;

            ++animalHunt;
        }

        /// <summary>
        /// 产生随机数
        /// </summary>
        /// <returns></returns>
        private int getRand()
        {
            int width = (int)this.PutAnimal.ActualWidth;
            int _sec = DateTime.Now.Second;

            Random rand = new Random(animalNum * _sec);

            return rand.Next(0,width - 10);
        }
        /// <summary>
        /// 移动窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_ForMove(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }
        
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult _isOk = MessageBox.Show("确认退出游戏？？？", "退出游戏", MessageBoxButton.OKCancel, 
                MessageBoxImage.Question, MessageBoxResult.Cancel);

            if(_isOk == MessageBoxResult.OK)
            {
                iFinish = false;
                this.Close();
            }
        }
    }
}
