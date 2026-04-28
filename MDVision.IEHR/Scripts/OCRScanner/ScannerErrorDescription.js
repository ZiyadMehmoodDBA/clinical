

//Error Handler for Scanner Lib SDK Errors

function ScannerErrors(value)
{
    switch(value)
    {
		case 1:
			alert("Scanner Lib : No Error");
			break; 
		case -1:
			alert("Scanner Lib : Invalid Scanner");
			break;
		case -2:
			alert("Scanner Lib : Scanner General Failed OR Bad Width Parameter");
			break;
		case -3:
			alert("Scanner Lib : Process Cancelled by User OR Bad Height Parameter");
			break;
		case -4:
			alert("Scanner Lib : Scanner Not Found");
			break;
		case -5:
			alert("Scanner Lib : Hardware Error");
			break;
		case -6:
			alert("Scanner Lib : Paper Fed Error");
			break;
		case -7:
			alert("Scanner Lib : Scan Aborted");
			break;
		case -8:
			alert("Scanner Lib : No Paper");
			break;
		case -9:
			alert("Scanner Lib : Paper Jam");
			break;
		case -10:
			alert("Scanner Lib : File Input/Output Error");
			break;			
		case -11:
			alert("Scanner Lib : Printer Port Used");
			break;
		case -12:
			alert("Scanner Lib : Out of Memory");
			break; 
		case -13:
			alert("Scanner Lib : Library Already Intialized");
			break; 
		case -14:
			alert("Scanner Lib : Driver not Found");
			break;
		case -15:
			alert("Scanner Lib : Scanner Busy");
			break;
		case -16:
			alert("Scanner Lib : Image Conversion Error");
			break;
		case -17:
			alert("Scanner Lib : Unload Failed due to Bad Parent");
			break;
		case -18:
			alert("Scanner Lib : Scanner Lib not Intialized");
			break;
		case -19:
			alert("Scanner Lib : Library already in use by another Application");
			break;				
		case -20:
			alert("Scanner Lib : License Key Expired OR Conflict with in/out Scan Parameters");
			break;			
		case -21:
			alert("Scanner Lib : Invalid License OR Conflict with Scan Size Parameters");
			break;
		case -22:
			alert("Scanner Lib : License Key does not match library OR No Support for Multiple Devices");
			break;
		case -23:
			alert("Scanner Lib : Camera already Intiliazed");
			break;
		case -24:
			alert("Scanner Lib : No Free Camera Found");
			break;
		case -25:
			alert("Scanner Lib : Camera not Found");
			break;
		case -26:
			alert("Scanner Lib : Camera not Assigned to this Application");
			break;
		case -27:
			alert("Scanner Lib : IPScan Version Too Old");
			break;
		case -28:
			alert("Scanner Lib : Asynchronous Scans in Queue");
			break;
			
		case -200:
			alert("Scanner Lib : Image Missing Stamp or SDK Missing Activation Code");
			break;
		case -201:
			alert("Scanner Lib : Scanner Already in Use");
			break;
		case -202:
			alert("Scanner Lib : Scanner Already in Use");
			break;
		case -203:
			alert("Scanner Lib : Cannot Open twain Source");
			break;
		case -204:
			alert("Scanner Lib : No Twain Installed");
			break;
		case -205:
			alert("Scanner Lib : No Next Value");
			break;		
		default:
			alert("Scanner Lib : Error - " + value);
    }
}

//Error Handler for Image Lib SDK Errors
function ImageErrors(value)
{
	switch(value)
	{
		case 0: 
			alert("Image Lib : No Error"); 
			break;
		case -100: //if error opening image file
			alert("Image Lib : Error in Opening File"); //prompt error message opening a file
			break;
		case -101: //if angle_0 is invalid
			alert("Image Lib : Bad Angle"); //prompt error message for invalid angle
			break;
		case -102: //if angle_1 is invalid
			alert("Image Lib : Bad Angle");//prompt error message for invalid angle
			break;
		case -103: //if destination is invalid
			alert("Image Lib : Bad Destination"); //prompt error message for invalid destination
			break;
		case -104: //if error saving to file
			alert("Image Lib : Error in Saving File"); //prompt error message for error saving to file
			break;
		case -105: //if error saving to clipboard
			alert("Image Lib : Error in Saving File to Clipboard"); //prompt error saving to clipboard
			break;
		case -106: //if error opening first file
			alert("Image Lib : Error in Opening File"); //prompt error opening first file
			break;
		case -107: //if error opening second file
			alert("Image Lib : Error in Opening File"); //prompt error opening second file
			break;
		case -108: //if error comb type
			alert("Image Lib : File Combo Type"); //prompt error message
			break;
		case -130: //if color is bad/invalid
			alert("Image Lib : Bad Color"); //prompt bad color
			break;
		case -131: //if dpi setting is invalid
			alert("Image Lib : Bad DPI"); //prompt bad setting of dpi
			break;
		case -132: //if internal image is invalid
			alert("Image Lib : Invalid Internal Image"); //prompt error message for invalid image
			break;
		 case -133:
			alert("Image Lib : Bad Image Dump");
			break;
		case -134:
			alert("Image Lib : Bad Image Dimensions"); 
			break;
		case -200: 
			alert("Image Lib : Image Missing Stamp or SDK Missing Activation Code");
			break;
		default:
			alert("Image Lib : ImageLib Error: " + value);
	}
}

//Error Handler for Activation of Twain Scanner on Machine
function ActivationErrors(value)
{
	switch (value)
	{
		case -1: //if activation key is invalid
			alert("Activation Lib : Invalid Activation Key");//prompt error message
			break;
		case -2: //can't find network card
			alert("Activation Lib : Cannot find network card");//prompt error message
			break;
		case -3: //activation key already used on another PC
			alert("Activation Lib : Activation failed. Activation key already used on another PC");//prompt error message
			break;
		case -4: //comm. error
			alert("Activation Lib : Communication error, unable to reach to the Activation server. Check firewall settings");//prompt error message
			break;		
		case -5: //can't create activation
			alert("Activation Lib : Cannot create activation"); //prompt error message
			break;
		case -11: //activation failed
			alert("Activation Lib : Activation Failed"); //prompt error message
			break;
		case -12: //activation failed
			alert("Activation Lib : Activation Failed"); //prompt error message
			break;
		default:
			alert("Activation Lib : Error - " + value);
	}
}

