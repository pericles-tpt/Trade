using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine.UI;

namespace Assets.Scripts.View.UI.Buttons.GameUI
{
    public class TurnTimeBehaviour : MonoBehaviour
    {
        float _mgt = 0f, _aqn = 0f, _shp = 0f, _sls = 0f;
        int minutes = 16 * 60;
        int barFull = 395;

        public void AdjustUsageBar(bool offsetMode = false, float mgt = 0, float aqn = 0, float shp = 0, float sls = 0)
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

        private void UpdateUI ()
        {
            float xDist = 0;
            float timeLeft = minutes - ((_mgt * minutes) + (_aqn * minutes) + (_shp * minutes) + (_sls * minutes));
            
            Text mgtTime = this.transform.FindChild("ManagementTime").gameObject.GetComponent<Text>();
            Text aqnTime = this.transform.FindChild("AcquisitionsTime").gameObject.GetComponent<Text>();
            Text shpTime = this.transform.FindChild("ShipmentsTime").gameObject.GetComponent<Text>();
            Text slsTime = this.transform.FindChild("SalesTime").gameObject.GetComponent<Text>();

            RectTransform mgtTimeUsed = (RectTransform)this.transform.FindChild("TimeUsedManagement").transform;
            RectTransform aqnTimeUsed = (RectTransform)this.transform.FindChild("TimeUsedAcquisitions").transform;
            RectTransform shpTimeUsed = (RectTransform)this.transform.FindChild("TimeUsedShipments").transform;
            RectTransform slsTimeUsed = (RectTransform)this.transform.FindChild("TimeUsedSales").transform;

            Vector3 BackgroundBarPos = this.transform.FindChild("TimeLeftBarBackground").transform.position;

            mgtTime.GetComponent<Text>().text = ConvertMinutesToText(minutes * _mgt);
            aqnTime.GetComponent<Text>().text = ConvertMinutesToText(minutes * _aqn);
            shpTime.GetComponent<Text>().text = ConvertMinutesToText(minutes * _shp);
            slsTime.GetComponent<Text>().text = ConvertMinutesToText(minutes * _sls);

            mgtTimeUsed.sizeDelta = new Vector2(barFull * _mgt, 48);
            mgtTimeUsed.transform.position = BackgroundBarPos;
            xDist += (barFull * _mgt);

            aqnTimeUsed.sizeDelta = new Vector2(barFull * _aqn, 48);
            aqnTimeUsed.transform.position = new Vector3(BackgroundBarPos.x + xDist, BackgroundBarPos.y, BackgroundBarPos.z);
            xDist += (barFull * _aqn);

            shpTimeUsed.sizeDelta = new Vector2(barFull * _shp, 48);
            shpTimeUsed.transform.position = new Vector3(BackgroundBarPos.x + xDist, BackgroundBarPos.y, BackgroundBarPos.z);
            xDist += (barFull * _shp);

            slsTimeUsed.sizeDelta = new Vector2(barFull * _sls, 48);
            slsTimeUsed.transform.position = new Vector3(BackgroundBarPos.x + xDist, BackgroundBarPos.y, BackgroundBarPos.z);

            this.transform.FindChild("EndTurnButton").FindChild("EndTurnText").GetComponent<Text>().text = "END TURN\n(" + ConvertMinutesToText(timeLeft) + ")";
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