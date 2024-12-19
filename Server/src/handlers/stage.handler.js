import { getGameAssets } from "../init/assets.js";
import { setStage, getStage } from '../models/stage.model.js';

export const moveStageHandler = (userId, payload) => {
    // 스테이지는 하나씩 올라간다.
    // 일정 점수가 되면 다음 스테이지로 이동한다.
    // 현재 스테이지 검증
    let currentStages = getStage(userId);
    if (!currentStages.length) {
        return { status: 'fail', message: 'no stages found' };
    }

    currentStages.sort((a, b) => a.id - b.id);
    const currentStage = currentStages[currentStages.length - 1];

    if (currentStage.id !== payload.currentStage) {
        return { status: 'fail', message: 'current stage not match' };
    }
    // 점수 검증
    const serverTime = Date.now();
    const elapsedTime = (serverTime - currentStage.timestamp) / 1000;

    // 지연시간이 5초 이상이면 에러 처리
    // TODO: 스테이지별로 조건 처리하기
    if (elapsedTime < 9 || elapsedTime > 12) {
        return { status: 'fail', message: 'invalid time' };
    }

    // 다음 스테이지 검증
    const { stages } = getGameAssets();
    if (!stages.data.some((stage) => stage.id === payload.targetStage)) {
        return { status: 'fail', message: 'target stage not found' };
    }

    setStage(userId, payload.targetStage, serverTime);

    return { status: "success" };
}

