fixed4 _Source1;
fixed4 _Source2;
fixed4 _Source3;
fixed4 _Source4;
fixed4 _Source5;
fixed4 _Source6;
fixed4 _Source7;
fixed4 _Source8;
fixed4 _Source9;
fixed4 _Source10;
fixed4 _Source11;
fixed4 _Source12;
fixed4 _Source13;
fixed4 _Source14;
fixed4 _Source15;
fixed4 _Source16;
fixed4 _Source17;
fixed4 _Source18;
fixed4 _Source19;
fixed4 _Source20;
fixed4 _Source21;
fixed4 _Source22;
fixed4 _Source23;
fixed4 _Source24;

fixed4 _Replace1;
fixed4 _Replace2;
fixed4 _Replace3;
fixed4 _Replace4;
fixed4 _Replace5;
fixed4 _Replace6;
fixed4 _Replace7;
fixed4 _Replace8;
fixed4 _Replace9;
fixed4 _Replace10;
fixed4 _Replace11;
fixed4 _Replace12;
fixed4 _Replace13;
fixed4 _Replace14;
fixed4 _Replace15;
fixed4 _Replace16;
fixed4 _Replace17;
fixed4 _Replace18;
fixed4 _Replace19;
fixed4 _Replace20;
fixed4 _Replace21;
fixed4 _Replace22;
fixed4 _Replace23;
fixed4 _Replace24;

float _Tolerance;

fixed3 ReplaceColor(fixed3 c, fixed4 source, fixed4 replace)
{
	fixed3 diff = abs(c.rgb - source.rgb);

	float factor = step(length(diff) / _Tolerance, 0.5);

	return lerp(c, replace, factor);
}

fixed3 ReplaceFirstTwentyFourColors(fixed3 color)
{
	color.rgb = ReplaceColor(color.rgb, _Source1, _Replace1);
	color.rgb = ReplaceColor(color.rgb, _Source2, _Replace2);
	color.rgb = ReplaceColor(color.rgb, _Source3, _Replace3);
	color.rgb = ReplaceColor(color.rgb, _Source4, _Replace4);
	color.rgb = ReplaceColor(color.rgb, _Source5, _Replace5);
	color.rgb = ReplaceColor(color.rgb, _Source6, _Replace6);
	color.rgb = ReplaceColor(color.rgb, _Source7, _Replace7);
	color.rgb = ReplaceColor(color.rgb, _Source8, _Replace8);
	color.rgb = ReplaceColor(color.rgb, _Source9, _Replace9);
	color.rgb = ReplaceColor(color.rgb, _Source10, _Replace10);
	color.rgb = ReplaceColor(color.rgb, _Source11, _Replace11);
	color.rgb = ReplaceColor(color.rgb, _Source12, _Replace12);
	color.rgb = ReplaceColor(color.rgb, _Source13, _Replace13);
	color.rgb = ReplaceColor(color.rgb, _Source14, _Replace14);
	color.rgb = ReplaceColor(color.rgb, _Source15, _Replace15);
	color.rgb = ReplaceColor(color.rgb, _Source16, _Replace16);
	color.rgb = ReplaceColor(color.rgb, _Source17, _Replace17);
	color.rgb = ReplaceColor(color.rgb, _Source18, _Replace18);
	color.rgb = ReplaceColor(color.rgb, _Source19, _Replace19);
	color.rgb = ReplaceColor(color.rgb, _Source20, _Replace20);
	color.rgb = ReplaceColor(color.rgb, _Source21, _Replace21);
	color.rgb = ReplaceColor(color.rgb, _Source22, _Replace22);
	color.rgb = ReplaceColor(color.rgb, _Source23, _Replace23);
	color.rgb = ReplaceColor(color.rgb, _Source24, _Replace24);

	return color;
}