docker build -t service.achievement . ;
docker tag service.achievement mateusz9090/achievement:local ;
docker push mateusz9090/achievement:local