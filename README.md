---
title: " ANALYZE WEBSITE CLICKSTREAM DATA  
 (Ryerson Capeston Project)"
author: "ShaimaaSoltan"
date: "October 31, 2015"
output: html_document
runtime: shiny
---
---
Objective
---

Clickstream data is an information trail a user leaves behind while visiting a website. It is typically captured in semi-structured website log files. Which contains Geographical data, Browsing Cookies Data and Time Line Data.
We can analyze this Data feeds to classify the website visitors According to their Loyalty to specific Site or specific Product, then we can recommend for those Users some Products with discount Capons, By Predicting which products do visitors tend to buy together.
First, we will Search for any malicious activity on this data feeds as a cleaning step, then classify the visitors into three main categories (Loyal, Guest, Normal) and according to their activity on the web site we can predict which products they tend to buy together.

<img src="Pics/Screenshot 2024-08-17 153257.png" />
<img src="Pics/Screenshot 2024-08-17 153310.png"/>
<img src="Pics/Screenshot 2024-08-17 153342.png" />
<img src="Pics/Screenshot 2024-08-17 153354.png" />
<img src="Pics/Screenshot 2024-08-17 153403.png"/>
<img src="Pics/Screenshot 2024-08-17 153415.png"/>
<img src="Pics/Screenshot 2024-08-17 153424.png"/>



## Data Preparation

You can download all RAW data files from [Git web site](https://github.com/shaimaasultan/ClickStream).

In this step we will Do `Exploratory Data Analysis (EDA)`
 BY Data exploration we will try to answers the questions like 
  what is the typical value of an attribute,
  how much the data points differ from the typical value,
  or are there any outliers in the data set.
  
## Step 1: Features Engineering & Exploratory Data Analysis (EDA).

```
# Cleaning an Labeling Some Featuers 

CREATE VIEW V_Visists
As
with vististCount As
(
SELECT IP , visit_Count = Count(visittimestamp) 
from dbo.Visits
group by IP
)

select v.*, c.visit_Count, u.BIRTH_DT,u.GENDER_CD ,GENDER= CASE WHEN u.GENDER_CD = 'M' THEN 1 
																  ELSE CASE WHEN  u.GENDER_CD = 'F' THEN 2
																  ELSE  CASE WHEN u.GENDER_CD = 'U' THEN 3
																  ELSE 4 END END END, url.category,
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
																  ELSE 16 END  
from ClearVisits v
JOIN vististCount c on c.IP = v.IP
left JOIN Users u ON u.SWID = v.UserID
Left JOIN [dbo].[URLMap] url ON url.url = v.URL
GO
```
  
```{r warning=FALSE, message=FALSE}
#1. Import the data	to R.
#------------------------------------------------
# Please First check your working directory first 
```
 
```{r}
df_data = read.csv(file='Visits.csv',header = T , sep = ',')
attach(df_data)
```

 ```{r, echo=FALSE} 
 nrow(df_data) 
 ``` 
 Rows.

```{r, echo = FALSE}
numericInput("rows", "How many Rows from Dataset?", 1)

renderTable({
  head(df_data, input$rows)
})
```


Distribution of Counting Unique visits 
---

```{r, echo=FALSE}
inputPanel(
  selectInput("n_breaks", label = "Number of bins:",
              choices = c(10, 20, 35, 50, 65, 85, 100), selected = 20),
  
  sliderInput("bw_adjust", label = "Bandwidth adjustment:",
              min = 0.2, max = 2, value = 1, step = 0.2)
)

renderPlot({
  hist(visit_Count, probability = TRUE, breaks = as.numeric(input$n_breaks),
       xlab = "Counting unique visits", main = "Distribution of Counting unique visits ")
  
  dens <- density(visit_Count, adjust = input$bw_adjust)
  lines(dens, col = "blue")
})
```


Distribution of Counting Unique visits By Product
---

```{r, echo=FALSE}
library('corrplot')
library('ggplot2')
renderPlot({
ggplot(df_data) + 
  geom_density(aes(x=visit_Count, colour=category),show_guide=FALSE)+
  stat_density(aes(x=visit_Count, colour=category),
               geom="line",position="identity")
})
```

Correlation Diagram Between Some Quantitative features
---

```{r, echo=FALSE}
corrplot(cor(df_data[c(1,2,3,4,5,6,28,31,36)]), order = "AOE", cl.ratio = 0.2, cl.align = "r")

```

kmeans cluster With Parameters 
---


```{r, echo=FALSE}

kmeans_cluster <- function(dataset) {

  shinyApp(
    ui = fluidPage(responsive = TRUE,
      fluidRow(style = "padding-bottom: 20px;",
        column(4, selectInput('xcol', 'X Variable',   c('IP_NUMBER','VisitTimeStamp','visit_Count','GENDER','ProductType'))),
        column(4, selectInput('ycol', 'Y Variable', c('IP_NUMBER','VisitTimeStamp','visit_Count','GENDER','ProductType'),
                              selected=names(dataset)[[23]])),
        column(4, numericInput('clusters', 'Cluster count', 4,
                               min = 1, max = 9))
      ),
      fluidRow(
        plotOutput('kmeans', height = "400px")
      )
    ),

    server = function(input, output, session) {

      # Combine the selected variables into a new data frame
      selectedData <- reactive({
        dataset[, c(input$ycol, input$xcol )]
      })

      clusters <- reactive({
        kmeans(selectedData(), input$clusters)
      })

      output$kmeans <- renderPlot(height = 400, {
        par(mar = c(5.1, 4.1, 0, 1))
        plot(selectedData(),
             col = clusters()$cluster,
             pch = 20, cex = 3)
        points(clusters()$centers, pch = 4, cex = 4, lwd = 4)
      })
    },

    options = list(height = 500)
  )
}
options(shiny.deprecation.messages=FALSE)
kmeans_cluster(df_data)
```
