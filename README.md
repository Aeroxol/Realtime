Socket.io를 활용한 소켓 프로그래밍 연습입니다.

### 개요
 우주 공간에서 날아오는 운석을 피해 오래 살아남으면 되는 간단한 게임입니다.

### 서버
 서버는 Node.js로 작성했습니다.
 
- 게임에 필요한 정보(스테이지, 아이템, 아이템 해금)를 json형태로 가지고 있습니다.
- 서버에 접속한 클라이언트의 UID, 스테이지에 관한 정보를 가지고 있습니다.
- 유저가 보낸 패킷을 packetId로 구분하여 적절한 핸들러로 처리합니다.

서버가 처리하는 핸들러는 아래와 같습니다.
- 게임 데이터 전송. 클라이언트가 접속하고 게임 데이터를 요청하면 서버에 있는 게임 데이터를 전송합니다.
- 게임 시작. 클라이언트가 게임 시작을 요청하면, 클라이언트의 UID와 게임 시작 정보를 저장합니다.
- 게임 종료. 클라이언트가 게임 종료를 요청하면, 클라이언트가 보낸 점수와 시간 정보로 데이터를 검증합니다.
- 스테이지 이동. 클라이언트가 일정 점수에 도달해 스테이지를 이동을 요청하면, 클라이언트가 보낸 점수와 시간 정보로 데이터를 검증합니다.
- 아이템 획득. 클라이언트가 아이템 획득을 요청하면, 현재 스테이지에 맞는 아이템 점수를 획득합니다.
- 채팅 메시지. 클라이언트가 채팅 메시지를 보내면, 모든 클라이언트에게 메시지를 브로드캐스트합니다.

### 클라이언트
 클라이언트는 Unity로 만들었습니다. Unity에는 공식 Socket.io 패키지가 없었기 때문에 비공식 Socket.io 패키지를 사용했습니다.
 https://bestdocshub.pages.dev/

 클라이언트는 게임 과정에서 서버에 패킷을 보내 요청하고, 응답을 받아 처리합니다.
 
 클라이언트가 처리하는 핸들러는 아래와 같습니다.
- 게임 데이터 요청. 게임을 시작하기 전에 서버에 게임 데이터를 요청합니다.
- 게임 시작 요청. 게임을 시작하기 위해 서버에 요청합니다. 서버로부터 응답이 오면 게임을 시작합니다.
- 게임 종료 요청. 게임을 종료하기 위해 서버에 요청합니다. 서버로부터 응답이 오면 게임을 종료합니다.
- 스테이지 이동 요청. 일정 점수에 도달하면 다음 스테이지로 이동합니다. 서버로부터 응답이 오면 스테이지를 이동합니다.
- 아이템 획득. 아이템을 획득하면 서버에 요청합니다. 서버로부터 응답이 오면 점수를 획득합니다.
- 채팅 메시지. 채팅창에 메시지를 입력하고 엔터를 누르거나 버튼을 누르면 메시지를 전송합니다. 전송된 메시지를 브로드캐스트 되어 모든 클라이언트에 전송됩니다.
