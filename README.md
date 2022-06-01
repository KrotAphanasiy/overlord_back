## Flash

### Contents

Projects:

* **Flash.Central.Api** - central api that receives data from recognition modules
* **Flash.Recognizer** - recognition module that reads camera stream and generates recognition events.

Requires PostgreSQL, libgdiplus

### How to run

Run API first, then start Recognizer.

### Tips

* Recognizer widely uses Strategy pattern. You can override any part of its functionality with mocks for debugging.
* Feel free to use `application.local.json` files that are not included into the repo but are supported by both Central and Recognizer.

### Warning

`docker-compose.yml` aims to run full development environment in a single command, but is not fully ready for now and will not work properly.


### Authorization

To authentificatite using camera policy you need to pass header "Camera", the value of the header should be <CAMERA_GUID_ID>:<BASE_64_ENCODED_CAMERA_KEY>

To get ready header value knowing camera id execute /api/v1/Demo/camerapassword/<CAMERA_GUID_ID>

## General architecture overview
The project consists of 5 main parts:
- Flash.Central.Api - the API to be used by detectors and recognizers
- Flash.Central.AdminApi - the API for administration panel website
- Recognizer (Python) - python service to recognize plate numbers on cut picture, taken from the camera frame by dertector
- Detector (Python) - python service, deployed on Jetson, that serves for finding, cutting and forwarding pictures to Flash.Central.Api
- Admin UI - the frontend part for administration panel
We are also using Kafka for queueing messages, got by Central.Api from Detector, for Recognizer, and PostgreSQL to store events, cameras etc. Entities relationship is going to be descriped in ER-diagram later

The whole application is held by Kubernetes, except Detector - Jetson is not envolved in our cluster. Kubernetes also helps to reinvoke services on faults.

### Flash.Central.Api
Serves to provide communication between Detector and Recognizer, stores and manages events, pictures and logs in database and on local disk. The core of the project also manages visits generation jobs and cleanup jobs for pictures and detection/recognition events. All vital parameters are passed in deployment config for Kubernetes.
The Api-layer provides quite simple interface of 3 vital endpoints:
- GET /api​/v1​/Camera​/{uid} - to let detector get camera data, authorize as a camera and allow to send events to Central
- POST /api/v1/DetectionEvent - endpoint for Detector to send DetectionEvent`s, which will be processed and put to Kafka
- POST /api/v1/RecognitionEvent - endpoint for Recognizer to send RecognitionEvent`s whith recognized plate numbers

Link to Swagger https://flash.devstaging.pw/swagger/index.html

### Flash.Central.AdminApi
Serves to provide API for Admin UI, mostly for CRUD operations in database. The Api reference is too long to be descriped there, so not to repeat, here`s the link to Swagger with API-documentation https://admin-flash.devstaging.pw/swagger/index.html

### Detector
Detector is a services created with Python in couple with Tensorflow and OpenCV to detect numeric plates in camera regions. At the moment it runs on Jetson-microcomputer, which`s going to be located at gas station just next to the camera. The algorithm is quite simple - first we get a frame from the camera, then we cut it for regions and try find a numeric plate in each region (mostly - in two regions)m using the neural network of MobileNet SSD architecture (find out more at https://github.com/tensorflow/models). After all processings the DetectionEvent object, that consists of original frame image, cropped image, timstamp and camera bindings, is sent to Flash.Central.Api. Detector is also faultproof - it means that if the link to Flash.Central.Api is lost in some reason, Detector will store all the events in local SQL Lite database and send all the events to server as soon as the link is reestablished. It's also supposed to make Detectr self-reenvokable - that means if simply the power on Jetson is off for some reason, the detector executable script is to be launched alongside with operation system as soon as it's boot again. Deployment is going to be held through Ansible. That's where the Detector's responsibility ends.

#### Detector launch guide
- pull the repository
- run "pip install -r requirements_filename.txt"
- cd to src/executable
- edit config.json if you need to augment some params or want to test detector in your own environment
- run "python plate-detector.py"
- enjoy the result

### Recognizer
Recognizer is a service created with python on the base of NomeroffNet open-source solution for licence plate numbers recognition (GitHub: https://github.com/ria-com/nomeroff-net). Unlike Detector, Recognizer is managed by Kubernetes, so it`s reevokable on default. Few words about fault-proofability - Recognizer doesn't store events in local storage, instead of that if sending event to Flash.Central.Api fails due to unexpected reason, Recognizer will try that again in exponentially enlarging time, what prevents it from spamming to Api. Due to the fact, that we can possibly have as much Recpgnizers as we want, thanks to Kafka, it seems to appear quite optimal and good solution. About actions sequence - firstly the DetectionEventMessage s taken from kafka message broker, then it's processed by nomeroff-net and forwarded to Flash.Central.Api if the recognized number passes regex and aspect ratio validation. It should also be mentioned, that the RecognitionEvent model being sent to server also consist detection event id, what allows us to mark the detection event as processed in the database.

### Recognizer launch guide (on local machine in native environment)
- pull the repository
- run "pip install -r requirements_filename.txt"
- in the project root execute "git clone https://github.com/ria-com/nomeroff-net" to pull the nomeroff-net project
- rename folder nomeroff-net to nomeroff_net (change dash to underscore), because python doesn`t like using dashes in package's names
- run "python setup.py build" and "python setup.py install" in nomeroff_net folder (if want to run on native)
- run "pip install -r nomeroff_net_requirements.txt" on the requirements file from nomeroff_net folder
- edit config.json if there`s any need for that
- cd to src/executable from project root
- run "python recognizer.py"
- enjoy the result

- AVAILABLE KAFKA BROKERS ARE VITAL FOR RECOGNIZER, IT WON`T LAUNCH WITH KAFKA BEING DOWN!


