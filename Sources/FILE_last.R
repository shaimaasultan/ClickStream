
is.installed <- function(mypkg) is.element(mypkg, installed.packages()[,1]) 
CalculateScore <- function(dt)
{
  
  for (i in 1 : nrow(dt))
  {
    SCore <-0;
    if (dt[i,'AGE'] > 0 && dt[i,'AGE'] <= 15 )
    {
      #chiled
      SCore <- SCore + 1;
    }else if(dt[i,'AGE'] > 15 && dt[i,'AGE'] <= 30)
    {
      #Young
      SCore <- SCore + 2;
    }else
    {
      #Oldman
      SCore <- SCore + 3;
    }
    
    if (dt[i,'Country'] == 'usa' )
    {
      SCore <- SCore + 100;
    }else if (dt[i,'Country'] == 'can')
    {
      SCore <- SCore + 200;
    }
    else if (dt[i,'Country'] == 'aus')
    {
      SCore <- SCore + 300;
    }
    else if (dt[i,'Country'] == 'fra')
    {
      SCore <- SCore + 400;
    }else
    {
      SCore <- SCore + 500;
    }
    
    if (dt[i,'GENDER'] == '1' )
    {
      SCore <- SCore + 10;
    }else if (dt[i,'GENDER'] == '2')
    {
      SCore <- SCore + 20;
    }
    else if (dt[i,'GENDER'] == '3')
    {
      SCore <- SCore + 30;
    }
    else
    {
      SCore <- SCore + 40;
    }
    
    dt[i,'Score'] <- SCore
  }
  return(dt)
}

ScoreMatrix <- function(dt)
{
  f<- factor(dt$Score)
  Rows <- levels(f)
  columns <- levels(factor(dt$category))
  df_score <- data.frame(c(1:length(Rows)),row.names = Rows)
  for (j in 1: length(Rows))
  {
    for (i in 1:length(columns))
    {
      S <-  nrow( subset(dt,dt$Score == Rows[j] & dt$category == columns[i]))
      total <-  nrow( subset(dt,dt$Score == Rows[j]))
      df_score[j,columns[i]] <- ( S / total ) * 100.00
    }
  }
  return(df_score)
}

discountMatrix <- function(dt)
{
  Rows <- levels(factor(dt$User_Class))
  columns <- levels(factor(dt$category))
  df_discount_score <- data.frame(c(1:length(Rows)),row.names = Rows)
  for (j in 1: length(Rows))
  {
    for (i in 1:length(columns))
    {
      S <-  nrow( subset(dt,dt$User_Class == Rows[j] & dt$category == columns[i]))
      
      total <-  nrow( subset(dt,dt$User_Class == Rows[j]))
      
      df_discount_score[j,columns[i]] <- floor(( S / total ) * 100)
       if (df_discount_score[j,columns[i]] > 30)
       {
         df_discount_score[j,columns[i]]<-30
       }
      
    }
  }
  df_discount_score <- df_discount_score[,-1]
  return(df_discount_score)
}

ProductList <-function(x)
{
  print(paste('Welcome To our website You are a ' , 
              if( is.na((dt[dt$IP == x,])$User_Class[1] ))
                  {
                    'GUEST'
                  }else
                  {
                    (dt[dt$IP == x,])$User_Class[1]
                  }
                  , ' USER '))
  print('Product List')
  print('--------------------')
  IPScore <- (dt[dt$IP == x ,])$Score[1] 
  if(!is.na(IPScore))
  {
    r<- df_score[as.character(IPScore),]
    r <- sort(r[,-1],decreasing = T)
    
    for(i in 1:ncol(r))
    {
      print(paste(colnames(r)[i]," --> ",floor(dt_discount[ (dt[dt$IP == x,])$User_Class[1],i]),"% Discount",sep=""))
    }
  }else
  {
    r<- df_score[as.character(543),]
    r <- sort(r[,-1],decreasing = T)
    
    for(i in 1:ncol(r))
    {
      print(colnames(r)[i])
    }
  }
}

ProductsIndex <- function(dt)
{
  Rows <- levels(factor(dt$Score))
  columns <- levels(factor(dt$category))
  df_discount_score <- data.frame(c(1:length(Rows)),row.names = Rows )
  for (j in 1:length(Rows))
  {
    prodIndex <- ''
    r<- df_score[as.character(Rows[j]),-c(1,14)]
    r <- order(r,decreasing = T)
    for (i in 1:length(r))
    {
      df_discount_score[j,i] <- r[[i]]
    }
    
  }
  colnames(df_discount_score) <- paste("Prod",c(1:(length(columns)-1)),sep = "")
  df_score<- cbind(df_score , df_discount_score)
  return(df_score)
}

panel.cor <- function(x, y, digits=2, prefix="", cex.cor, ...)
{
  usr <- par("usr"); on.exit(par(usr))
  par(usr = c(0, 1, 0, 1))
  r <- abs(cor(x, y))
  txt <- format(c(r, 0.123456789), digits=digits)[1]
  txt <- paste(prefix, txt, sep="")
  if(missing(cex.cor)) cex.cor <- 0.8/strwidth(txt)
  text(0.5, 0.5, txt, cex = cex.cor * r)
}
-------------------------------------------------------------------------------------

df_data = read.csv(file='Visits.csv',header = T , sep = ',')
set.seed(4)

if (!is.installed('sqldf'))
{
  install.packages('sqldf')
}

library(sqldf)

df_train <- df_data[sample(1:nrow(df_data),nrow(df_data) * .7),]
df_test <-df_data[sample(1:nrow(df_data),nrow(df_data) * .3),]
dt_Class <- sqldf('SELECT Count(distinct USERID) AS NumberOfSessions , Visit_Count,IP_NUMBER,
         Count(distinct domain) AS Domain , (strftime("%s",max(visitdate)) - strftime("%s",  min(visitdate))) / 60 AS SessionDuration , IP,Count(URL) AS URL FROM df_train
        GROUP BY IP,IP_NUMBER,Visit_Count ')
dt_Class2 <- sqldf('SELECT Count(distinct USERID) AS NumberOfSessions,Visit_Count,IP_NUMBER,
         Count(distinct domain) AS Domain , (strftime("%s",max(visitdate)) - strftime("%s",  min(visitdate))) / 60 AS SessionDuration , IP,Count(URL) AS URL ," " AS Class   FROM df_test
                   GROUP BY IP,Visit_Count')





# If number of sessions for the same IP > 4 in less than 24 hours then we consider it as not normal activity
Class <- ifelse( (dt_Class$SessionDuration < 1440 & dt_Class$NumberOfSessions >= 4 ) , "NotNormal" , "Normal")
dt_Class$Class <- NULL
dt_Class <-  data.frame(dt_Class,Class)


if (!is.installed('C50'))
{
  install.packages('C50')
}
library('C50')

m1 <- C5.0(dt_Class[,-ncol(dt_Class)],dt_Class[,ncol(dt_Class)])

plot(m1,main='IP Activity Classification Model Using C5.0 Decision Tree')
summary(m1)

Predict_model <- predict(m1,dt_Class2,type='class')
dt_Class2$Class <- Predict_model



df_Normal <- subset(dt_Class2,dt_Class2$Class == 'Normal')

# level 2 Filter
dt_Class <- subset(dt_Class , dt_Class$Class == "Normal")
Class <- ifelse( (dt_Class$visit_Count > 300 &  dt_Class$NumberOfSessions == 1 )|  (  dt_Class$URL / dt_Class$SessionDuration ) > 0.9  , "NotNormal" , "Normal")

dt_Class$Class <- NULL
dt_Class <- data.frame(dt_Class,Class)

if (!is.installed('C50'))
{
  install.packages('C50')
}
library('C50')

m1 <- C5.0(dt_Class[,-ncol(dt_Class)],dt_Class[,ncol(dt_Class)])

plot(m1,main='IP Activity Classification Model Using C5.0 Decision Tree')
df_Normal$Class <- '' 
Predict_model <- predict(m1,df_Normal,type='class')
df_Normal$Class <- Predict_model

df_Normal <- subset(df_Normal,df_Normal$Class == 'Normal')


df_Normal_Full <- sqldf('SELECT IP ,GENDER , GENDER_CD,category,TVShow,
                        City,Country,StreetNumber,State,TV_Station
,IP_Class,User_Class,Visis_Day,Day_visit_Count,DayVisitRatio,IPVisitRatio
,Language,BIRTH_DT, IP_NUMBER,
(strftime("%Y", "now") - strftime("%Y", BIRTH_DT)) 
     - (strftime("%m-%d", "now") < strftime("%m-%d", BIRTH_DT) ) AS AGE
                        FROM df_test where IP in (select IP from df_Normal)')
plot(df_Normal_Full$category,df_Normal_Full$Visis_Day , col = df_Normal_Full$GENDER)

legend('topleft','GROUP',c('1 (Male)','2 (Female)','3 (U)','4 (NA)') , col=c('Green','RED','BLUE','YELLOW'), lty = 1)

dt <- df_Normal_Full[,c('IP', 'IP_NUMBER','AGE' , 'Country' ,'GENDER')]


dt <- CalculateScore(df_Normal_Full)

rm(df_test)
rm(df_train)
rm(dt_Class)
rm(dt_Class2)
rm(df_Normal)
---------------------------------------------------
  
set.seed(300)
df_train <- dt[sample(1:nrow(dt),nrow(dt) * .7),]
df_test <-dt[sample(1:nrow(dt),nrow(dt) * .3),]

df_train <- CalculateScore(df_train)

dt_train_Classes <- df_train['Score']
df_test$Score <- ''


dt_discount <- discountMatrix(dt)
df_score <- ScoreMatrix(dt)
ProductList('173.255.176.213')
ProductList('67.224.130.63')

ProductList('67.224.0.6')

df_score <- ProductsIndex(dt)
pairs(~ df_score$Prod1 + df_score$Prod2 + df_score$Prod3+ df_score$Prod4 + df_score$Prod5 +
        df_score$Prod6 + df_score$Prod7 + df_score$Prod8+ df_score$Prod9 + df_score$Prod10 +
        df_score$Prod11 + df_score$Prod12 ,
      data=df_score, lower.panel=panel.smooth, upper.panel=panel.cor, 
      pch=20, main="Product Order Scatterplot Matrix")
#---------------------------

#----------------------------------------------------------
#Module 2) - Apply knn from Class package
#==========================================================
#load calss
library(class)
is_missing_data <- any(is.infinite(df_test))
#Apply knn
ml<- knn(train = df_train , test=df_test , cl = as.factor(df_train[,'Score']),k=3)

#test Performance
conf_Matrix <- table(df_test[,'Score'] , ml)
#number of correct predict = sum(diagonal in conf matrix)
Correct_Predict <- 0;
for (i in 1:nrow(conf_Matrix))
{
  Correct_Predict <- Correct_Predict + conf_Matrix[i,i];
}
# number of false predicted = total - correct predected
False_pred <- nrow(df_test) - Correct_Predict;

Accuracy_Perc <- (Correct_Predict / nrow(df_test) ) * 100;
Error_Perc <-  (False_pred / nrow(df_test))* 100;
---------------------------------------------------

rm(x)
rm(dt_evaluate)

Rows <- levels(factor(dt$Score))
columns <- levels(factor(dt$category))
dt_evaluate <- cbind(dt[Rows[1] == dt$Score, c('IP_NUMBER','AGE','Score','GENDER')],
                     df_score[Rows[1],c(15:ncol(df_score))])
for(i in 2:length(Rows))
{
 x <- cbind(dt[Rows[i] == dt$Score, c('IP_NUMBER','AGE','Score','GENDER')],
    df_score[Rows[i],c(15:ncol(df_score))])
 dt_evaluate <- rbind(dt_evaluate,x)
}
#--------------------------

lm.model <-lm(cbind(dt_evaluate$Prod1,dt_evaluate$Prod2,dt_evaluate$Prod3,dt_evaluate$Prod4,
                    dt_evaluate$Prod5,dt_evaluate$Prod6,dt_evaluate$Prod7,dt_evaluate$Prod8,
                    dt_evaluate$Prod9,dt_evaluate$Prod10,dt_evaluate$Prod11,dt_evaluate$Prod12) ~ dt_evaluate$IP_NUMBER + dt_evaluate$AGE + dt_evaluate$Score + dt_evaluate$GENDER)

lm.model <-lm(dt_evaluate$Prod1 ~ dt_evaluate$IP_NUMBER + dt_evaluate$AGE + dt_evaluate$Score + dt_evaluate$GENDER)
#------------------------------------------------------
install.packages("RWeka")
install.packages("kknn")
library("RWeka") # rweka (embedded Weka software)
library("kknn") # knn library
#-------------------------------------------------------------------
# Module 1) - Apply Cross-validation on combined DataSet on Weka KNN
#-------------------------------------------------------------------
#create weka Classifier
Classifier <- IBk(as.factor(dt_evaluate$Prod1) ~ dt_evaluate$IP_NUMBER + dt_evaluate$AGE + dt_evaluate$Score + dt_evaluate$GENDER , data=dt_evaluate, control= Weka_control(K = 3))
#Evalute IBK Classifier using cross validation
evaluate_Weka_classifier(Classifier,numFolds =20,data = dt_evaluate)

#-------------
rm(x)
rm(dt_evaluate)

Rows <- levels(factor(df_train$Score))
columns <- levels(factor(df_train$category))
dt_evaluate <- cbind(df_train[Rows[1] == df_train$Score, c('IP_NUMBER','AGE','Score','GENDER')],
                     df_score[Rows[1],c(15:ncol(df_score))])
for(i in 2:length(Rows))
{
  x <- cbind(df_train[Rows[i] == df_train$Score, c('IP_NUMBER','AGE','Score','GENDER')],
             df_score[Rows[i],c(15:ncol(df_score))])
  dt_evaluate <- rbind(dt_evaluate,x)
}
#-----------------------------------------------
# Module 4) SVM algorthim
#-----------------------------------------------
install.packages('e1071')
library(e1071)
X <- dt_evaluate[,-c(5:16)]
Y<- factor(dt_evaluate[,5])
DF <- data.frame(dt_evaluate[,c(1:5)])
svm_model <- svm(Y ~ DF$IP_NUMBER + DF$AGE + DF$Score + DF$GENDER , data = DF, method = "C-classification",
                  kernel = "radial", cost = 5, gamma = 0.5)
summary(svm_model)
rm(df_data)
rm(x)
rm(X)
rm(df_Normal_Full)
plot(svm_model, DF, DF$Score ~ DF$GENDER, slice = list(DF$Score , DF$GENDER))

#Predection Classes Table
conf_Matrix <- table(Y,predict(svm_model,DF))
#number of correct predict = sum(diagonal in conf matrix)
Correct_Predict <- 0;
for (i in 1:nrow(conf_Matrix))
{
  Correct_Predict <- Correct_Predict + conf_Matrix[i,i];
}
# number of false predicted = total - correct predected
False_pred <- nrow(DF) - Correct_Predict;

Accuracy_Perc <- (Correct_Predict / nrow(DF) ) * 100;
Error_Perc <-  (False_pred / nrow(DF))* 100;
----------------------------------------------------
rm(x)
rm(dt_evaluate)

Rows <- levels(factor(df_test$Score))
columns <- levels(factor(df_test$category))
dt_evaluate <- cbind(df_test[Rows[1] == df_test$Score, c('IP_NUMBER','AGE','Score','GENDER')],
                     df_score[Rows[1],c(15:ncol(df_score))])
for(i in 2:length(Rows))
{
  x <- cbind(df_test[Rows[i] == df_test$Score, c('IP_NUMBER','AGE','Score','GENDER')],
             df_score[Rows[i],c(15:ncol(df_score))])
  dt_evaluate <- rbind(dt_evaluate,x)
}

DF <- data.frame(dt_evaluate[,c(1:5)])
Y<- factor(dt_evaluate[,5])
----------------------------------------------------
# see the overfitting 
conf_Matrix <- table(Y,predict(svm_model,DF))
#number of correct predict = sum(diagonal in conf matrix)
Correct_Predict <- 0;
for (i in 1:nrow(conf_Matrix))
{
  Correct_Predict <- Correct_Predict + conf_Matrix[i,i];
}
# number of false predicted = total - correct predected
False_pred <- nrow(DF) - Correct_Predict;

Accuracy_Perc <- (Correct_Predict / nrow(DF) ) * 100
Error_Perc <-  (False_pred / nrow(DF))* 100
--------------------------------------
# On the second product
  
DF <- data.frame(dt_evaluate[,c(1,2,3,4,6)])
Y<- factor(dt_evaluate[,6])
----------------------------------------------------
# see the overfitting 
conf_Matrix <- table(Y,predict(svm_model,DF))
#number of correct predict = sum(diagonal in conf matrix)
Correct_Predict <- 0;
for (i in c(1:5))
{
  Correct_Predict <- Correct_Predict + conf_Matrix[i,i];
}
# number of false predicted = total - correct predected
False_pred <- nrow(DF) - Correct_Predict;

Accuracy_Perc <- (Correct_Predict / nrow(DF) ) * 100
Error_Perc <-  (False_pred / nrow(DF))* 100
