cd /d "%~dp0"
set CP="C:\Users\chien\Downloads\Làng Lá - Server\out\production\pkoolvn";"C:\Users\chien\Downloads\Làng Lá - Server\lib\*"
javac -cp %CP% PrintTask.java
java -cp ".;%CP%" PrintTask

