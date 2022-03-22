﻿/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XCharts.Examples
{
    [DisallowMultipleComponent]
    public class Example30_PieChart : MonoBehaviour
    {
        private PieChart chart;
        private Serie serie, serie1;
        private float m_RadiusSpeed = 100f;
        private float m_CenterSpeed = 1f;

        void Awake()
        {
            LoopDemo();
        }

        private void OnEnable()
        {
            LoopDemo();
        }

        void LoopDemo()
        {
            StopAllCoroutines();
            StartCoroutine(PieDemo());
        }

        IEnumerator PieDemo()
        {
            StartCoroutine(PieAdd());
            yield return new WaitForSeconds(2);
            StartCoroutine(PieShowLabel());
            yield return new WaitForSeconds(4);
            StartCoroutine(Doughnut());
            yield return new WaitForSeconds(3);
            StartCoroutine(DoublePie());
            yield return new WaitForSeconds(2);
            StartCoroutine(RosePie());
            yield return new WaitForSeconds(5);
            LoopDemo();
        }

        IEnumerator PieAdd()
        {
            chart = gameObject.GetComponent<PieChart>();
            if (chart == null) chart = gameObject.AddComponent<PieChart>();
            chart.title.text = "PieChart - 饼图";
            chart.title.subText = "基础饼图";

            chart.legend.show = true;
            chart.legend.location.align = Location.Align.TopLeft;
            chart.legend.location.top = 60;
            chart.legend.location.left = 2;
            chart.legend.itemWidth = 70;
            chart.legend.itemHeight = 20;
            chart.legend.orient = Orient.Vertical;

            chart.RemoveData();
            serie = chart.AddSerie(SerieType.Pie, "访问来源");
            serie.radius[0] = 0;
            serie.radius[1] = 110;
            serie.center[0] = 0.5f;
            serie.center[1] = 0.4f;
            chart.AddData(0, 335, "直接访问");
            chart.AddData(0, 310, "邮件营销");
            chart.AddData(0, 243, "联盟广告");
            chart.AddData(0, 135, "视频广告");
            chart.AddData(0, 1548, "搜索引擎");
            chart.RefreshLabel();

            chart.onPointerClickPie = delegate(PointerEventData e, int serieIndex, int dataIndex){

            };
            yield return new WaitForSeconds(1);
        }

        IEnumerator PieShowLabel()
        {
            chart.title.subText = "显示文本标签";

            serie.label.show = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
            serie.label.lineType = SerieLabel.LineType.Curves;
            chart.RefreshChart();

            yield return new WaitForSeconds(1);
            serie.label.lineType = SerieLabel.LineType.HorizontalLine;
            chart.RefreshChart();

            yield return new WaitForSeconds(1);
            serie.label.lineType = SerieLabel.LineType.BrokenLine;
            chart.RefreshChart();

            yield return new WaitForSeconds(1);
            serie.label.show = false;
            chart.RefreshChart();
        }

        IEnumerator Doughnut()
        {
            chart.title.subText = "圆环图";
            serie.radius[0] = 2f;
            while (serie.radius[0] < serie.radius[1] * 0.7f)
            {
                serie.radius[0] += m_RadiusSpeed * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
            serie.pieSpace = 1f;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            serie.data[0].selected = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            serie.pieSpace = 0f;
            serie.data[0].selected = false;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }

        IEnumerator DoublePie()
        {
            chart.title.subText = "多图组合";

            serie1 = chart.AddSerie(SerieType.Pie, "访问来源2");
            chart.AddData(1, 335, "直达");
            chart.AddData(1, 679, "营销广告");
            chart.AddData(1, 1548, "搜索引擎");
            serie1.radius[0] = 0;
            serie1.radius[1] = 2f;
            serie1.center[0] = 0.5f;
            serie1.center[1] = 0.4f;
            chart.RefreshChart();
            while (serie1.radius[1] < serie.radius[0] * 0.75f)
            {
                serie1.radius[1] += m_RadiusSpeed * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }

            serie1.label.show = true;
            serie1.label.position = SerieLabel.Position.Inside;
            serie1.label.textStyle.color = Color.white;
            serie1.label.textStyle.fontSize = 14;
            serie1.label.border = false;

            chart.RefreshLabel();
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }

        IEnumerator RosePie()
        {
            chart.title.subText = "玫瑰图";
            chart.legend.show = false;
            serie1.ClearData();
            serie.ClearData();
            serie1.radius = serie.radius = new float[2] { 0, 80 };
            serie1.label.position = SerieLabel.Position.Outside;
            serie1.label.lineType = SerieLabel.LineType.Curves;
            serie1.label.textStyle.color = Color.clear;
            for (int i = 0; i < 2; i++)
            {
                chart.AddData(i, 10, "rose1");
                chart.AddData(i, 5, "rose2");
                chart.AddData(i, 15, "rose3");
                chart.AddData(i, 25, "rose4");
                chart.AddData(i, 20, "rose5");
                chart.AddData(i, 35, "rose6");
                chart.AddData(i, 30, "rose7");
                chart.AddData(i, 40, "rose8");
            }

            while (serie.center[0] > 0.25f || serie1.center[0] < 0.7f)
            {
                if (serie.center[0] > 0.25f) serie.center[0] -= m_CenterSpeed * Time.deltaTime;
                if (serie1.center[0] < 0.7f) serie1.center[0] += m_CenterSpeed * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
            yield return new WaitForSeconds(1);
            while (serie.radius[0] > 3f)
            {
                serie.radius[0] -= m_RadiusSpeed * Time.deltaTime;
                serie1.radius[0] -= m_RadiusSpeed * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }

            serie.radius[0] = 0;
            serie1.radius[0] = 0;
            serie.pieRoseType = RoseType.Area;
            serie1.pieRoseType = RoseType.Radius;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }
    }
}