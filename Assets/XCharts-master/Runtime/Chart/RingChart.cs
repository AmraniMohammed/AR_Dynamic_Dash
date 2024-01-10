using UnityEngine;

namespace XCharts.Runtime
{
    [AddComponentMenu("XCharts/pieChart", 20)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class pieChart : BaseChart
    {
        protected override void DefaultChart()
        {
            GetChartComponent<Tooltip>().type = Tooltip.Type.Line;
            RemoveData();
            Ring.AddDefaultSerie(this, GenerateDefaultSerieName());
        }
    }
}