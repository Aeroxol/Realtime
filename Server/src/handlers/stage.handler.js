import { getGameAssets } from "../init/assets.js";
import { setStage, getStage } from '../models/stage.model.js';

export const moveStageHandler = (uuid, payload) => {
    // 스테이지는 하나씩 올라간다.
    // 일정 점수가 되면 다음 스테이지로 이동한다.
    // 현재 스테이지 검증

    let currentStages = getStage(uuid).stages;
    if (!currentStages.length) {
        console.log('no stages found');
        return { status: 'fail', message: 'no stages found' };
    }

    currentStages.sort((a, b) => a.id - b.id);
    const currentStage = currentStages[currentStages.length - 1];

    if (currentStage.id !== payload.currentStage) {
        console.log('current stage not match');
        return { status: 'fail', message: 'current stage not match' };
    }

    // 점수 검증
    const userStages = getStage(uuid).stages;
    const itemScore = getStage(uuid).score;

    let totalScore = 0;
    let serverTime = Date.now();

    userStages.forEach((stage, index) => {
        if (index === userStages.length - 1) {
            serverTime = Date.now();
        } else {
            serverTime = userStages[index + 1].timestamp;
        }
        const stageDuration = (serverTime - stage.timestamp);
        totalScore += stageDuration; // 1초당 1점
    })
    totalScore += itemScore;

    const { stages } = getGameAssets();

    console.log(stages);
    if (stages.data.find((e) => e.id === currentStage.id).score > totalScore + 5) {
        console.log("score verification failed");
        return { status: 'fail', message: 'score verification failed' };
    }
    console.log('move stage score validation success');

    // const serverTime = Date.now();
    // const elapsedTime = (serverTime - currentStage.timestamp) / 1000;

    // 지연시간이 5초 이상이면 에러 처리
    // TODO: 스테이지별로 조건 처리하기
    // if (elapsedTime < 9 || elapsedTime > 12) {
    //     return { status: 'fail', message: 'invalid time' };
    // }

    // 다음 스테이지 검증
    if (!stages.data.some((stage) => stage.id === payload.targetStage)) {
        return { status: 'fail', message: 'target stage not found' };
    }

    setStage(uuid, payload.targetStage, serverTime);

    console.log('move stage success');
    return { status: 'success', packetId: 11, payload: JSON.stringify(payload.targetStage) };

}

