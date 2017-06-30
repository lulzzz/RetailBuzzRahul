$(document).ready(function () {
    alert("hudhuhua")
                var height1 = $(document).height();// height of full document
                	if ( $.browser.msie ) {
                           height1 = Math.abs(height1 + 157);
                           }

					globalHeight = height1;
					var height2 = $("#header").height(); // height of header
					var height_diff = Math.abs(height1 - height2) + "px";
					
					/* global variable - it can be used anywhere in the page*/
					globalHeightDiff = height_diff;
					document.getElementById('container').style.height = height_diff; // Set the remaining height in container DIV.
					$('#footer').css('margin-top', height1+ 105);
					 
					});
