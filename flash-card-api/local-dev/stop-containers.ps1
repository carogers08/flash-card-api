echo "Stopping and removing containers"

docker stop (docker ps -a -q)
echo "All containers stopped"
docker rm (docker ps -a -q)
echo "All containers removed"