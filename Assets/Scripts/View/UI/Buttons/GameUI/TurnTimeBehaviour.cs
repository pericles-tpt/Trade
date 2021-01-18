using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.View.UI.Buttons.GameUI
{
    public class TurnTimeBehaviour : MonoBehaviour
    {
        float _mgt = 0f, _aqn = 0f, _shp = 0f, _sls = 0f;
        int minutes = 16 * 60;
        int barFull = 395;

        public void AdjustUsageBarPercentage(float mgt = 0, float aqn = 0, float shp = 0, float sls = 0, bool offsetMode = false)
        {
            if (offsetMode)
            {
                _mgt += mgt;
                _aqn += aqn;
                _shp += shp;
                _sls += sls;
            } else
            {
                _mgt = mgt;
                _aqn = aqn;
                _shp = shp;
                _sls = sls;
            }

            UpdateUI();

        }

        public void AdjustUsageBarMinutes(int mgtMins = 0, int aqnMins = 0, int shpMins = 0, int slsMins = 0, bool offsetMode = false)
        {
            if (offsetMode)
            {
                _mgt += (mgtMins / minutes);
                _aqn += (aqnMins / minutes);
                _shp += (shpMins / minutes);
                _sls += (slsMins / minutes);
            }
            else
            {
                _mgt = (mgtMins / minutes);
                _aqn = (aqnMins / minutes);
                _shp = (shpMins / minutes);
                _sls = (slsMins / minutes);
            }

            UpdateUI();

        }

        private void UpdateUI ()
        {
            float xDist = 0;
            int pixelToUnits = 36;
            int barHeight = 48;
            float timeLeft = minutes - ((_mgt * minutes) + (_aqn * minutes) + (_shp * minutes) + (_sls * minutes));
            
            // UPDATING TEXT UI ELEMENTS
            Text mgtTime = this.transform.FindChild("TimeLegend").transform.FindChild("ManagementTime").gameObject.GetComponent<Text>();
            Text aqnTime = this.transform.FindChild("TimeLegend").transform.FindChild("AcquisitionsTime").gameObject.GetComponent<Text>();
            Text shpTime = this.transform.FindChild("TimeLegend").transform.FindChild("ShipmentsTime").gameObject.GetComponent<Text>();
            Text slsTime = this.transform.FindChild("TimeLegend").transform.FindChild("SalesTime").gameObject.GetComponent<Text>();

            mgtTime.GetComponent<Text>().text = ConvertMinutesToText(minutes * _mgt);
            aqnTime.GetComponent<Text>().text = ConvertMinutesToText(minutes * _aqn);
            shpTime.GetComponent<Text>().text = ConvertMinutesToText(minutes * _shp);
            slsTime.GetComponent<Text>().text = ConvertMinutesToText(minutes * _sls);

            this.transform.FindChild("EndTurnButton").FindChild("EndTurnText").GetComponent<Text>().text = "END TURN\n(" + ConvertMinutesToText(timeLeft) + " LEFT)";

            // UPDATING PROGRESS BAR UI ELEMENTS
            RectTransform mgtTimeUsed = (RectTransform)this.transform.FindChild("TimeBars").transform.FindChild("TimeUsedManagement").transform;
            RectTransform aqnTimeUsed = (RectTransform)this.transform.FindChild("TimeBars").transform.FindChild("TimeUsedAcquisitions").transform;
            RectTransform shpTimeUsed = (RectTransform)this.transform.FindChild("TimeBars").transform.FindChild("TimeUsedShipments").transform;
            RectTransform slsTimeUsed = (RectTransform)this.transform.FindChild("TimeBars").transform.FindChild("TimeUsedSales").transform;

            Vector3 BackgroundBarPos = this.transform.FindChild("TimeBars").transform.FindChild("TimeLeftBarBackground").transform.position;

            mgtTimeUsed.sizeDelta = new Vector2(barFull * _mgt, barHeight);
            mgtTimeUsed.transform.position = BackgroundBarPos;
            xDist += mgtTimeUsed.sizeDelta.x;

            aqnTimeUsed.sizeDelta = new Vector2(barFull * _aqn, barHeight);
            aqnTimeUsed.transform.position = new Vector3(BackgroundBarPos.x + xDist/pixelToUnits, BackgroundBarPos.y, BackgroundBarPos.z);
            xDist += aqnTimeUsed.sizeDelta.x;

            shpTimeUsed.sizeDelta = new Vector2(barFull * _shp, barHeight);
            shpTimeUsed.transform.position = new Vector3(BackgroundBarPos.x + xDist/pixelToUnits, BackgroundBarPos.y, BackgroundBarPos.z);
            xDist += shpTimeUsed.sizeDelta.x;

            slsTimeUsed.sizeDelta = new Vector2(barFull * _sls, barHeight);
            slsTimeUsed.transform.position = new Vector3(BackgroundBarPos.x + xDist/pixelToUnits, BackgroundBarPos.y, BackgroundBarPos.z);

        }

        private string ConvertMinutesToText(float minutes)
        {
            int asInt = Mathf.RoundToInt(minutes);
            int m = asInt % 60;
            int h = (int)Mathf.Floor(asInt / 60);

            string mPart, hPart;
            if (m < 10)
                mPart = "0" + m + "M";
            else
                mPart = m + "M";

            if (h < 10)
                hPart = "0" + h + "H";
            else
                hPart = h + "H";

            return hPart + " " + mPart;
        }
    }
}