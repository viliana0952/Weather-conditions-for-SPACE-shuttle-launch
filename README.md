# Weather-conditions-for-SPACE-shuttle-launch
Analysing the space shuttle launch conditions with regard to weather and location

Your task is to analyse the space shuttle launch conditions with regard to weather and location.
You need to find the most appropriate date/location for the space shuttle launch based on the
weather criteria.
You have the weather forecast for the first half of July for every of the given locations and the
weather criteria for a successful space shuttle launch.
The application takes 4 input parameters – Folder name (path to the folder
on the file system, containing the weather forecast for every spaceport), Sender email
address, Password, Receiver email address.
The type of the accepted input file for the weather forecast (contained in the
folder) is CSV.
The criteria for the weather conditions for a space shuttle launch is as follows:
Temperature between 1 and 32 degrees Celsius;
Wind speed no more than 11m/s (the lower the better);
Humidity less than 55% (the lower the better);
No precipitation;
No lightings;
No cumulus or nimbus clouds.
The application should calculate the following:
for each spaceport – find the best date for space shuttle launch;
find the best combination of date and location, if location is considered as better, the closer it is to the
Equator.
based on the above criteria and create a new CSV file named “LaunchAnalysisReport.csv” containing two
columns – the first one is the name of the spaceport, the second one is the result for the best date for the
given spaceport.
The newly generated CSV file (1) should be sent to the email (4th
input parameter). The body of the email should contain the best
combination of date and location for the space shuttle launch (2).
This will happen by using the 2nd and 3rd input parameters
(Sender mail and Password) to establish a connection using
SMTP and send the file as an attachment to the email. Hint: using
Gmail SMTP could be difficult because they have additional
security. Try other services like Outlook, for example.
Bonus tasks:
Make the application UI multilingual (English &
German) with the ability to change the language.
Allow weather criteria (part or all of it) to be
entered as input parameters to enable more
flexibility. Also, think about the possibility of having
different weather criteria for the different
spaceports.


