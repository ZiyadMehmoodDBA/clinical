//
// Dynamsoft JavaScript Library for Basic Initiation of Dynamic Web TWAIN
// More info on DWT: http://www.dynamsoft.com/Products/WebTWAIN_Overview.aspx
//
// Copyright 2017, Dynamsoft Corporation 
// Author: Dynamsoft Team
// Version: 13.0
//
/// <reference path="dynamsoft.webtwain.initiate.js" />
var Dynamsoft = Dynamsoft || { WebTwainEnv: {} };

Dynamsoft.WebTwainEnv.AutoLoad = true;
var w_ = localStorage.DWT_width ? localStorage.DWT_width : 800;
var h_ = localStorage.DWT_height ? localStorage.DWT_height : 600;

Dynamsoft.WebTwainEnv.Containers = [{ ContainerId: 'dwtcontrolContainer', Width: w_, Height: h_ }];
///
Dynamsoft.WebTwainEnv.ProductKey = 'f0068MgAAALyRnL/f1M6NHUuTLL+y3PpDPwglBjyW53AG4E5FX0ZsD2ljXAd6RyvwFz0ljSZeRAa+J2+KRe8QMq3dTFNz6XE=';
///
Dynamsoft.WebTwainEnv.Trial = false;
///
Dynamsoft.WebTwainEnv.ActiveXInstallWithCAB = false;
///
// Dynamsoft.WebTwainEnv.ResourcesPath = 'Resources';

/// All callbacks are defined in the dynamsoft.webtwain.install.js file, you can customize them.

// Dynamsoft.WebTwainEnv.RegisterEvent('OnWebTwainReady', function(){
// 		// webtwain has been inited
// });

