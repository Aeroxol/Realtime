import { getGameAssets } from '../init/assets.js';
import { setStage, getStage, clearStage } from '../models/stage.model.js';

var hiscore = 0;

export const gameStart = (uuid, payload) => {
    const { stages } = getGameAssets();

    clearStage(uuid);
    // 클라이언트의 정보를 신뢰해서는 안됨. 시간관계상 그냥 저장.
    setStage(uuid, stages.data[0].id, payload.timestamp);
    console.log('Stage: ', getStage(uuid).stages);

    return { status: 'success', packetId: 2 };
}

export const gameEnd = (uuid, payload) => {
    console.log('gameEnd packet received!');
    // 클라이언트는 게임 종료 시 타임스탬프와 총 점수
    const { timestamp: gameEndTime, score } = payload;
    const stages = getStage(uuid).stages;
    const itemScore = getStage(uuid).score;

    if (!stages.length) {
        console.log("no stages found for user");
        return { status: 'fail', message: 'no stages found for user' };
    }
    // 각 스테이지의 지속시간을 계산하여 총 점수 계산
    let totalScore = 0;

    stages.forEach((stage, index) => {
        let stageEndTime;
        if (index === stages.length - 1) {
            stageEndTime = gameEndTime;
        } else {
            stageEndTime = stages[index + 1].timestamp;
        }
        const stageDuration = (stageEndTime - stage.timestamp);
        totalScore += stageDuration; // 1초당 1점
    })
    totalScore += itemScore;

    clearStage(uuid);

    if (Math.abs(score - totalScore) > 5) {
        console.log("score verification failed");
        console.log(`expected: ${totalScore}, actual: ${score}`);
        return { status: 'fail', message: 'score verification failed' };
    }

    if (score > hiscore) {
        hiscore = score;
        return { status: 'success', packetId: 3, payload: 'HISCORE', message: 'game ended' };
    } else {
        // DB에 저장
        return { status: 'success', packetId: 3, message: 'game ended' };
    }
}

export const getItem = (uuid, payload) => {
    const { score } = payload;
    // {score, [{id, timestamp}]}
    const stages = getStage(uuid);
    stages.score += score;
    return { status: 'success', packetId: 22 };
}