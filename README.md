# Log Proxy Api
## Instructions to install and run the Log Proxy API
1. Download and unzip "LogProxyAPI.zip"
1. Load the “LogProxyAPI.tar”-file to docker using “docker load -I \<path to image tar file\>” command
1. Run image in docker (e.g.: “docker run -d -p 8080:80 --name logProxyApi logproxyapi”)
1. Access website
	* GET/POST to …/messages
	* Add Basic Authentication header using user=logProxy password=logProxy
	* Try out .../swagger to see API documentation
