        var CSSN_NONE = 0;
        var CSSN_600 = 1;
        var CSSN_800 = 2;
        var CSSN_800N = 3;
        var CSSN_1000 = 4;
        var CSSN_2000 = 5;
        var CSSN_2000N = 6;
        var CSSN_800E = 7;
        var CSSN_800EN = 8;
        var CSSN_3000 = 9;
        var CSSN_4000 = 10;
        var CSSN_TWAIN = 11;
        var CSSN_5000 = 12;
        var CSSN_IDR = 13;  // SnapShell - OCR only
        var CSSN_800DX = 14;
        var CSSN_800DXN = 15;
        var CSSN_FDA = 16;  // SnapShell - OCR + Digimark watermark verification
        var CSSN_WMD = 17;   // SnapShell - Digimark watermar verification only
        var CSSN_TWN = 18;   // SnapShell - General twain camera
        var CSSN_PASS = 19;   // SnapShell - Passport camera
        var CSSN_RTE8K = 20;   
        var CSSN_TWAIN_N = 21;
        var CSSN_MAGTEK_STX = 22;
        var CSSN_CLBS = 23;
        var CSSN_IP = 24;
        var CSSN_1000N = 25;
        var CSSN_3000DN = 26;
        var CSSN_900DX = 27;
        var CSSN_RTE9K = 28;
        var CSSN_3100 = 29;
        var CSSN_3100N = 30;
		var CSSN_R2 = 33;
	 
		// Newly added scanners
		var CSSN_PASS_R2 = 38;
		var CSSN_ROW = 31; // snapshell - raw
        var CSSN_PASS_N = 32; // snapshell passport N
        var CSSN_LIGHT_IDR = 34;
        var CSSN_LIGHT_IDR72 = 35;
        var CSSN_LIGHT_PAS = 36;
        var CSSN_LIGHT_SCN = 37;
        var CSSN_IDR72N = 39;
        var CSSN_PASS_DBL_72N = 40;
        var CSSN_IDR72_DBL_DPLX = 41;
		
		function IsScannerDuplex(scanner_type)
        {
            var ret=false;
            if (scanner_type == CSSN_900DX || scanner_type == CSSN_3000 || scanner_type==CSSN_800DX || scanner_type==CSSN_800DXN || scanner_type==CSSN_3000DN || scanner_type==CSSN_3100 || scanner_type==CSSN_3100N || scanner_type==CSSN_IDR72_DBL_DPLX )
            {
                ret=true;
            }
            return ret;
        }

        function IsScannerOCR(scanner_type)
        {
            var ret = true;
            if(scanner_type==CSSN_800N || scanner_type==CSSN_2000N || scanner_type==CSSN_800EN || scanner_type==CSSN_800DXN || scanner_type==CSSN_TWAIN_N || scanner_type==CSSN_1000N || scanner_type==CSSN_3100N)
            {
                ret=false;
            }
            return ret;
        }
		
		function IsCalibrationRequired(scanner_num)
		{
			var ret = false;
  		    if (scanner_num ==  CSSN_600 || scanner_num ==  CSSN_800 || scanner_num ==  CSSN_800N || scanner_num ==  CSSN_1000 || scanner_num ==  CSSN_2000 || scanner_num ==  CSSN_2000N || scanner_num ==  CSSN_800E || scanner_num ==  CSSN_800EN || scanner_num ==  CSSN_3000 || scanner_num ==  CSSN_800DX || scanner_num ==  CSSN_800DXN || scanner_num ==  CSSN_IP || scanner_num ==  CSSN_1000N || scanner_num ==  CSSN_3000DN || scanner_num ==  CSSN_3100 || scanner_num ==  CSSN_3100N)
			{
				 ret=true;
			}
			return ret;
		}
		
		   function GetScannerNameByType(ScannerType)
            {
            var m_ScannerModel = "";

            switch (ScannerType)
            {
                case CSSN_600:
                    m_ScannerModel = "ScanShell 600";
                    break;
                case CSSN_800:
                    m_ScannerModel = "ScanShell 800R";
                    break;
                case CSSN_800N:
                    m_ScannerModel = "ScanShell 800NR";
                    break;
                case CSSN_1000:
                    m_ScannerModel = "ScanShell 1000";
                    break;
                case CSSN_2000:
                    m_ScannerModel = "ScanShell 2000R";
                    break;
                case CSSN_2000N:
                    m_ScannerModel = "ScanShell2000 NR";
                    break;
                case CSSN_800E:
                    m_ScannerModel = "ScanShell 800E";
                    break;
                case CSSN_800EN:
                    m_ScannerModel = "ScanShell 800 EN";
                    break;
                case CSSN_3000:
                    m_ScannerModel = "ScanShell 3000";
                    break;
                case CSSN_4000:
                    m_ScannerModel = "Fujistu fi60";
                    break;
                case CSSN_5000:
                    m_ScannerModel = "Bancor";
                    break;
                case CSSN_IDR:
                    m_ScannerModel = "SnapShell IDR";
                    break;
                case CSSN_800DX:
                    m_ScannerModel = "ScanShell 800DX";
                    break;
                case CSSN_800DXN:
                    m_ScannerModel = "ScanShell 800DXN";
                    break;
                case CSSN_FDA:
                    m_ScannerModel = "SnapShell FDA";
                    break;
                case CSSN_WMD:
                    m_ScannerModel = "SnapShell WMD";
                    break;
                case CSSN_TWN:
                    m_ScannerModel = "SnapShell TWN";
                    break;
                case CSSN_PASS:
                    m_ScannerModel = "SnapShell Passport";
                    break;
                case CSSN_RTE8K:
                    m_ScannerModel = "RTE 8000";
                    break;
                case CSSN_TWAIN_N:
                    m_ScannerModel = "Twain N";
                    break;
                case CSSN_MAGTEK_STX:
                    m_ScannerModel = "Magtek STX";
                    break;
                case CSSN_CLBS:
                    m_ScannerModel = "SnapShell Clb.";
                    break;
                case CSSN_IP:
                    m_ScannerModel = "ScanShell IP";
                    break;

                case CSSN_1000N:
                    m_ScannerModel = "ScanShell 1000N";
                    break;

                case CSSN_3000DN:
                    m_ScannerModel = "ScanShell 3000DN";
                    break;

                case CSSN_900DX:
                    m_ScannerModel = "ScanShell 900DX";
                    break;

                case CSSN_RTE9K:
                    m_ScannerModel = "AT 9000";
                    break;

                case CSSN_3100:
                    m_ScannerModel = "ScanShell 3100";
                    break;
                case CSSN_3100N:
                    m_ScannerModel = "ScanShell 3100N";
                    break;
				case CSSN_R2:
                    m_ScannerModel = "SnapShell R2";
                    break;
			
			// Newly added scanners			
                case CSSN_PASS_R2:
                    m_ScannerModel = "SnapShell Passport R2";
                    break;

                case CSSN_LIGHT_IDR:
                    m_ScannerModel = "SnapShell Light";
                    break;
                case CSSN_LIGHT_IDR72:
                    m_ScannerModel = "SnapShell R2 Light";
                    break;
                case CSSN_LIGHT_PAS:
                    m_ScannerModel = "SnapShell Passport Light";
                    break;
                case CSSN_LIGHT_SCN:
                    m_ScannerModel = "SnapShell SCN";
                    break;
                case CSSN_IDR72N:
                    m_ScannerModel = "SnapShell R2 N";
                    break;
                case CSSN_PASS_DBL_72N:
                    m_ScannerModel = "SnapShell Passport dbl N";
                    break;
                case CSSN_IDR72_DBL_DPLX:
                    m_ScannerModel = "SnapShell Duplex";
                    break;

                case CSSN_NONE:
                    m_ScannerModel = "None";
                    break;
								
            }
            return m_ScannerModel;
        }
			
		
		
// JavaScript Document


