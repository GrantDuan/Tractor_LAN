using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Kuaff.CardResouces;

namespace Duan.Xiugang.Tractor
{
    class CalculateRegionHelper
    {
        MainForm mainForm;
        internal CalculateRegionHelper(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }


        /// <summary>
        /// �����Ƿ���������
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        /// <param name="clicks">1 for one click, 2 for double click</param>
        /// <returns></returns>
        internal bool CalculateClickedRegion(MouseEventArgs e,int clicks)
        {
            bool result = false;
            //�����������
            Region[] regions = new Region[mainForm.myCardsLocation.Count];
            for (int i = 0; i < mainForm.myCardsLocation.Count; i++)
            {
                if ((bool)mainForm.myCardIsReady[i])
                {
                    regions[i] = new Region(new Rectangle((int)mainForm.myCardsLocation[i], 355, 71, 96));
                }
                else
                {
                    regions[i] = new Region(new Rectangle((int)mainForm.myCardsLocation[i], 375, 71, 96));
                }
            }

            //���㽻��,���һ���϶����ᱻ����,����5���Ḳ�ǵ�0��
            int k = mainForm.myCardsLocation.Count;
            int m = 0;
            if (mainForm.myCardsLocation.Count > 5)
            {
                for (int i = 0; i < mainForm.myCardsLocation.Count - 5; i++)
                {
                    regions[i].Exclude(regions[i + 1]);
                    regions[i].Exclude(regions[i + 2]);
                    regions[i].Exclude(regions[i + 3]);
                    regions[i].Exclude(regions[i + 4]);
                    regions[i].Exclude(regions[i + 5]);
                }

                m = mainForm.myCardsLocation.Count - 5;
                k = 5;
            }

            for (int i = 0; i < k - 1; i++)
            {
                for (int j = 1; j < (k - i); j++)
                {
                    regions[i + m].Exclude(regions[m + i + j]);
                }
            }


            //���������Ƿ�����������
            for (int i = 0; i < mainForm.myCardsLocation.Count; i++)
            {
                if (regions[i].IsVisible(e.X, e.Y))
                {
                    if (clicks == 2)
                    {
                        mainForm.myCardIsReady[i] = true;
                    }
                    else if (clicks == 1)
                    {
                        mainForm.myCardIsReady[i] = !(bool)mainForm.myCardIsReady[i];
                    }
                    result = true;
                    break;
                }
            }

            mainForm.SelectedCards.Clear();
            for (int i = 0; i < mainForm.myCardIsReady.Count; i++)
            {
                if ((bool)mainForm.myCardIsReady[i])
                {
                    mainForm.SelectedCards.Add((int)mainForm.myCardsNumber[i]);
                }
            }

            return result;
        }

        //�����Ƿ���������
        internal bool CalculateDoubleClickedRegion(MouseEventArgs e)
        {
            //�����������
            Region[] regions = new Region[mainForm.myCardsLocation.Count];
            for (int i = 0; i < mainForm.myCardsLocation.Count; i++)
            {
                if ((bool)mainForm.myCardIsReady[i])
                {
                    regions[i] = new Region(new Rectangle((int)mainForm.myCardsLocation[i], 355, 71, 96));
                }
                else
                {
                    regions[i] = new Region(new Rectangle((int)mainForm.myCardsLocation[i], 375, 71, 96));
                }
            }

            //���㽻��,���һ���϶����ᱻ����,����5���Ḳ�ǵ�0��
            int k = mainForm.myCardsLocation.Count;
            int m = 0;
            if (mainForm.myCardsLocation.Count > 5)
            {
                for (int i = 0; i < mainForm.myCardsLocation.Count - 5; i++)
                {
                    regions[i].Exclude(regions[i + 1]);
                    regions[i].Exclude(regions[i + 2]);
                    regions[i].Exclude(regions[i + 3]);
                    regions[i].Exclude(regions[i + 4]);
                    regions[i].Exclude(regions[i + 5]);
                }

                m = mainForm.myCardsLocation.Count - 5;
                k = 5;
            }

            for (int i = 0; i < k - 1; i++)
            {
                for (int j = 1; j < (k - i); j++)
                {
                    regions[i + m].Exclude(regions[m + i + j]);
                }
            }


            //���������Ƿ�����������
            for (int i = 0; i < mainForm.myCardsLocation.Count; i++)
            {
                if (regions[i].IsVisible(e.X, e.Y))
                {
                    mainForm.myCardIsReady[i] = true;
                    
                    return true;
                }
            }

            return false;
        }


        internal int CalculateRightClickedRegion(MouseEventArgs e)
        {
            //�����������
            Region[] regions = new Region[mainForm.myCardsLocation.Count];
            for (int i = 0; i < mainForm.myCardsLocation.Count; i++)
            {
                if ((bool)mainForm.myCardIsReady[i])
                {
                    regions[i] = new Region(new Rectangle((int)mainForm.myCardsLocation[i], 355, 71, 96));
                }
                else
                {
                    regions[i] = new Region(new Rectangle((int)mainForm.myCardsLocation[i], 375, 71, 96));
                }
            }

            //���㽻��,���һ���϶����ᱻ����,����5���Ḳ�ǵ�0��
            int k = mainForm.myCardsLocation.Count;
            int m = 0;
            if (mainForm.myCardsLocation.Count > 5)
            {
                for (int i = 0; i < mainForm.myCardsLocation.Count - 5; i++)
                {
                    regions[i].Exclude(regions[i + 1]);
                    regions[i].Exclude(regions[i + 2]);
                    regions[i].Exclude(regions[i + 3]);
                    regions[i].Exclude(regions[i + 4]);
                    regions[i].Exclude(regions[i + 5]);
                }

                m = mainForm.myCardsLocation.Count - 5;
                k = 5;
            }

            for (int i = 0; i < k - 1; i++)
            {
                for (int j = 1; j < (k - i); j++)
                {
                    regions[i + m].Exclude(regions[m + i + j]);
                }
            }


            //���������Ƿ�����������
            for (int i = 0; i < mainForm.myCardsLocation.Count; i++)
            {
                if (regions[i].IsVisible(e.X, e.Y))
                {
                    mainForm.myCardIsReady[i] = !(bool)mainForm.myCardIsReady[i];
                    return i;
                }
            }

            return -1;
        }
    }
}
