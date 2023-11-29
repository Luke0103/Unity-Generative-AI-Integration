# Unity-Generative-AI-Integration
생성형 AI를 Unity Engine에 시범 적용하는 프로젝트입니다.
(README.md 마지막 수정: 2023-11-29)

# Stable Diffusion
## 개요
AUTOMATIC1111의 stable-diffusion-webui를 서버로 하여금 Unity 환경에서 Stable Diffusion을 사용할 수 있도록 한다.

tex2img, img2img를 지원하며 현재 서버에 등록된 Model 중 하나를 선택 및 프롬프트 작성 가능하다.

stable-diffusion-webui의 사용법은 [해당 프로젝트 페이지](https://github.com/AUTOMATIC1111/stable-diffusion-webui) 참조 바라며, API 사용을 위해 COMMAND_ARGS에 --api는 필히 추가되어야 함.

API 문서는 `{해당 서버의 주소}/docs`에서 확인 가능하며, 예시로 로컬 호스트 사용 중일 경우 [http://127.0.0.1:7860/docs](http://127.0.0.1:7860/docs)가 된다.

Scene은 ImageEdit Scene을 사용 중이다.

## 클래스 구조도
![제목 없음-2023-06-01-1839](https://github.com/Luke0103/Unity-Generative-AI-Integration/assets/38881169/236a577e-361f-425f-b05b-167de1614375)

### 설명
 * Stable Diffusion Settings

AUTOMATIC1111 서버에 접속할 시 필요한 URL 등 데이터를 저장하는 ScriptableObject로, Server URL(예시: http:127.0.0.1:7860) 및 기타 API에 대한 링크, 여러 기본값을 포함한다.

 * Stable Diffusion Configuration

서버 내의 Model 리스트를 검색하여 이를 토대로 Scene의 UI 초기화하고, 다른 모델로 전환할 것을 서버에 요청하는 역할을 담당한다.

이외에도 Stable Diffusion Settings의 인스턴스를 비롯한 핵심 데이터들을 저장하는 역할을 수행하며, 타 클래스들에서 사용하게 된다.

 * Stable Diffusion Generator

사용자 입력에 따라 특정 Model, Sampler, Prompt 등을 담아 서버에 전달하며, 결과값을 전달받는 역할을 수행한다.

요청 종류에 따라 파생 클래스인 Stable Diffusion Text2Image, Stable Diffusion Image2Image 클래스를 사용하게 된다.

## 기능
### Text to Text
Prompt, Negative Prompt를 입력 가능하며 생성 요청 시 서버 측에서 해당 프롬프트에 맞는 이미지를 선택된 모델로 반환, 이를 우측 Image 컴포넌트를 통해 표시함

서버에 넘겨줄 때 `Texture2D.EncodeToPNG()`를 사용하며 서버에서 넘겨받을 때는 `Texture2D.LoadImage()`를 사용

### Image to Text
2023-11-29 기준 좌측 Image 컴포넌트에 원하는 이미지 파일을 할당하고 Prompt, Negative Prompt를 작성하여 서버에 전달, 이를 토대로 결과 이미지를 전달 받는다
