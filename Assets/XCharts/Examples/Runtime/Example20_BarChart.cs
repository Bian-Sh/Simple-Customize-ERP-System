﻿/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections;
using UnityEngine;

namespace XCharts.Examples
{
    [DisallowMultipleComponent]
    public class Example20_BarChart : MonoBehaviour
    {
        private BarChart chart;
        private Serie serie, serie2;
        private int m_DataNum = 5;

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
            StartCoroutine(AddSimpleBar());
            yield return new WaitForSeconds(2);
            StartCoroutine(BarMutilSerie());
            yield return new WaitForSeconds(3);
            StartCoroutine(ZebraBar());
            yield return new WaitForSeconds(3);
            StartCoroutine(SameBarAndNotStack());
            yield return new WaitForSeconds(3);
            StartCoroutine(SameBarAndStack());
            yield return new WaitForSeconds(3);
            StartCoroutine(SameBarAndPercentStack());
            yield return new WaitForSeconds(10);

            LoopDemo();
        }

        IEnumerator AddSimpleBar()
        {
            chart = gameObject.GetComponent<BarChart>();
            if (chart == null) chart = gameObject.AddComponent<BarChart>();
            chart.title.text = "BarChart - 柱状图";
            chart.title.subText = "普通柱状图";

            chart.yAxis0.minMaxType = Axis.AxisMinMaxType.Default;

            chart.RemoveData();
            serie = chart.AddSerie(SerieType.Bar, "Bar1");

            for (int i = 0; i < m_DataNum; i++)
            {
                chart.AddXAxisData("x" + (i + 1));
                chart.AddData(0, UnityEngine.Random.Range(30, 90));
            }
            yield return new WaitForSeconds(1);
        }


        IEnumerator BarMutilSerie()
        {
            chart.title.subText = "多条柱状图";

            float now = serie.barWidth - 0.35f;
            while (serie.barWidth > 0.35f)
            {
                serie.barWidth -= now * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }

            serie2 = chart.AddSerie(SerieType.Bar, "Bar2");
            serie2.lineType = LineType.Normal;
            serie2.barWidth = 0.35f;
            for (int i = 0; i < m_DataNum; i++)
            {
                chart.AddData(1, UnityEngine.Random.Range(20, 90));
            }
            yield return new WaitForSeconds(1);
        }

        IEnumerator ZebraBar()
        {
            chart.title.subText = "斑马柱状图";
            serie.barType = BarType.Zebra;
            serie2.barType = BarType.Zebra;
            serie.barZebraWidth = serie.barZebraGap = 4;
            serie2.barZebraWidth = serie2.barZebraGap = 4;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }

        IEnumerator SameBarAndNotStack()
        {
            chart.title.subText = "非堆叠同柱";
            serie.barType = serie2.barType = BarType.Normal;
            serie.stack = "";
            serie2.stack = "";
            serie.barGap = -1;
            serie2.barGap = -1;
            chart.RefreshAxisMinMaxValue();
            yield return new WaitForSeconds(1);
        }

        IEnumerator SameBarAndStack()
        {
            chart.title.subText = "堆叠同柱";
            serie.barType = serie2.barType = BarType.Normal;
            serie.stack = "samename";
            serie2.stack = "samename";
            chart.RefreshAxisMinMaxValue();
            yield return new WaitForSeconds(1);
            float now = 0.6f - serie.barWidth;
            while (serie.barWidth < 0.6f)
            {
                serie.barWidth += now * Time.deltaTime;
                serie2.barWidth += now * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
            serie.barWidth = serie2.barWidth;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }

        IEnumerator SameBarAndPercentStack()
        {
            chart.title.subText = "百分比堆叠同柱";
            serie.barType = serie2.barType = BarType.Normal;
            serie.stack = "samename";
            serie2.stack = "samename";

            serie.barPercentStack = true;

            serie.label.show = true;
            serie.label.position = SerieLabel.Position.Center;
            serie.label.border = false;
            serie.label.textStyle.color = Color.white;
            serie.label.formatter = "{d:f0}%";

            serie2.label.show = true;
            serie2.label.position = SerieLabel.Position.Center;
            serie2.label.border = false;
            serie2.label.textStyle.color = Color.white;
            serie2.label.formatter = "{d:f0}%";

            chart.RefreshLabel();
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }
    }
}