USE [ClickStream]
GO

/****** Object:  View [dbo].[V_Visists]    Script Date: 2015-10-31 8:56:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[V_Visists]
As
with vististCount As
(
SELECT IP , visit_Count = Count(visittimestamp)
from dbo.Visits
group by IP
)
, VisitDayCount AS
(
SELECT VisitDay = DAY(visitDate) , Day_visit_Count = Count(visittimestamp) 
from dbo.Visits
group by DAY(visitDate)
)

select IP_NUMBER = DENSE_RANK()  OVER( order  by v.IP), Octet1 ,Octet2,Octet3,Octet4 , v.*, c.visit_Count, u.BIRTH_DT,u.GENDER_CD ,
GENDER= CASE WHEN u.GENDER_CD = 'M' THEN 1 
		WHEN  u.GENDER_CD = 'F' THEN 2
		WHEN u.GENDER_CD = 'U' THEN 3
		ELSE 4 END , url.category,
IP_Class = CASE WHEN  c.visit_Count between 1 and 100 then 'A'
				WHEN c.visit_Count between 101 and 200 then 'B'
				WHEN c.visit_Count between 201 and 300 then 'C'
				WHEN c.visit_Count between 301 and 400 then 'D'
				WHEN c.visit_Count between 401 and 550 then 'E'
				ELSE 'F' END,
User_Class =  CASE WHEN  c.visit_Count between 1 and 100 then 'GEUST'
				WHEN c.visit_Count between 101 and 200 then 'NOrmaL'
				WHEN c.visit_Count between 201 and 300 then 'ACTIVE'
				WHEN c.visit_Count >300 then 'LOYAL'
				END,
IP_Public_Private = CASE WHEN  Octet1 = 10 
					AND Octet2 between 0 and 255 
					AND Octet3 between 0 and 255
					AND Octet4 between 0 and 255 then 'Private'
			WHEN  Octet1 = 172 
					AND Octet2 between 16 and 31 
					AND Octet3 between 0 and 255
					AND Octet4 between 0 and 255 then 'Private'
			WHEN  Octet1 = 192 
					AND Octet2 = 168
					AND Octet3 between 0 and 255
					AND Octet4 between 0 and 255 then 'Private'
			ELSE 'Public' END,
ProductType =  CASE WHEN url.category = 'clothing' THEN 1 
				WHEN  url.category = 'accessories' THEN 2
				WHEN url.category = 'automotive' THEN 3
				WHEN url.category = 'books' THEN 4
				WHEN url.category = 'clothing' THEN 5
				WHEN url.category = 'computers' THEN 6
				WHEN url.category = 'electronics' THEN 7
				WHEN url.category = 'games' THEN 8
				WHEN url.category = 'grocery' THEN 9
				WHEN url.category = 'handbags' THEN 10
				WHEN url.category = 'home&garden' THEN 11
				WHEN url.category = 'movies' THEN 12
				WHEN url.category = 'outdoors' THEN 13
				WHEN url.category = 'shoes' THEN 14
				WHEN url.category = 'tools' THEN 15
				ELSE 16 END,
Visis_Day =  Day(VisitDate), dc.Day_visit_Count,
TotalVisitCount = (select SUM(distinct visit_Count) from vististCount)					 
from ClearVisits v
CROSS APPLY dbo.fn_ParseText2Table(v.IP,'.') AS octet
JOIN vististCount c on c.IP = v.IP
JOIN VisitDayCount dc ON dc.VisitDay = Day(VisitDate)
left JOIN Users u ON u.SWID = v.UserID
Left JOIN [dbo].[URLMap] url ON url.url = v.URL
GO 

alter View V_Featuer_visits
AS
select *
	, DayVisitRatio = (Convert(decimal ,Day_visit_Count )/ (Select Count(*) from   [V_Visists]) )
	, IPVisitRatio = (Convert(decimal ,visit_Count) / TotalVisitCount )    
from [V_Visists]

GO


ALTER   function fn_ParseText2Table 
 (
 @p_SourceText  varchar(8000)
 ,@p_Delimeter varchar(100) = ',' --default to comma delimited.
 )
RETURNS @retOctet TABLE 
 (
  Octet1  int 
 ,Octet2 int 
 ,Octet3 int
 ,Octet4 int
 ,IP varchar(50)
 )
AS
BEGIN
 DECLARE @w_Continue  int
  ,@w_StartPos  int
  ,@w_Length  int
  ,@w_Delimeter_pos int
  ,@w_tmp_int  int
  ,@w_tmp_num  numeric(18,3)
  ,@w_tmp_txt   varchar(2000)
  ,@w_Delimeter_Len tinyint

declare @retTable TABLE 
 (
  Position  int identity(1,1)
 ,Int_Value int 

 )
 if len(@p_SourceText) = 0
 begin
  SET  @w_Continue = 0 -- force early exit
 end 
 else
 begin
 -- parse the original @p_SourceText array into a temp table
  SET  @w_Continue = 1
  SET @w_StartPos = 1
  SET @p_SourceText = RTRIM( LTRIM( @p_SourceText))
  SET @w_Length   = DATALENGTH( RTRIM( LTRIM( @p_SourceText)))
  SET @w_Delimeter_Len = len(@p_Delimeter)
 end
 WHILE @w_Continue = 1
 BEGIN
  SET @w_Delimeter_pos = CHARINDEX( @p_Delimeter
      ,(SUBSTRING( @p_SourceText, @w_StartPos
      ,((@w_Length - @w_StartPos) + @w_Delimeter_Len)))
      )
 
  IF @w_Delimeter_pos > 0  -- delimeter(s) found, get the value
  BEGIN
   SET @w_tmp_txt = LTRIM(RTRIM( SUBSTRING( @p_SourceText, @w_StartPos 
        ,(@w_Delimeter_pos - 1)) ))
   if isnumeric(@w_tmp_txt) = 1
   begin
    set @w_tmp_int = cast( cast(@w_tmp_txt as numeric) as int)
    set @w_tmp_num = cast( @w_tmp_txt as numeric(18,3))
   end
   else
   begin
    set @w_tmp_int =  null
    set @w_tmp_num =  null
   end
   SET @w_StartPos = @w_Delimeter_pos + @w_StartPos + (@w_Delimeter_Len- 1)
  END
  ELSE -- No more delimeters, get last value
  BEGIN
   SET @w_tmp_txt = LTRIM(RTRIM( SUBSTRING( @p_SourceText, @w_StartPos 
      ,((@w_Length - @w_StartPos) + @w_Delimeter_Len)) ))
   if isnumeric(@w_tmp_txt) = 1
   begin
    set @w_tmp_int = cast( cast(@w_tmp_txt as numeric) as int)
    set @w_tmp_num = cast( @w_tmp_txt as numeric(18,3))
   end
   else
   begin
    set @w_tmp_int =  null
    set @w_tmp_num =  null
   end
   SELECT @w_Continue = 0
  END
  INSERT INTO @retTable VALUES( @w_tmp_int, @w_tmp_num, @w_tmp_txt,@p_SourceText )
  
 END
 INSERT INTO @retOctet select Octet1 = (select Int_Value from @retTable where Position = 1),
Octet2 = (select Int_Value from @retTable where Position = 2),
Octet3 = (select Int_Value from @retTable where Position = 3),
Octet4 = (select Int_Value from @retTable where Position = 4),
IP = @p_SourceText
RETURN
END
GO
