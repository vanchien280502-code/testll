@echo off
cd /d "C:\Users\chien\Downloads\langla-291\scratch"
set "CLASSPATH=C:\Users\chien\Downloads\Làng Lá - Server\out\production\pkoolvn;C:\Users\chien\Downloads\Làng Lá - Server\lib\*"
javac -encoding UTF-8 -cp "%CLASSPATH%" PrintTask.java
java -cp ".;%CLASSPATH%" PrintTask

