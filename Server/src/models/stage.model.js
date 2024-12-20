// stages
// {
//     uuid : {score: int, stages: [ { id, timestamp }, { id, timestamp }]}
// }
const stages = {};

export const createStage = (uuid) => {
    stages[uuid] = { score: 0, stages: [] };
}

export const getStage = (uuid) => {
    return stages[uuid];
}

// uuid: 유저 id, id: 스테이지 id, timestamp: 타임스탬프(시작 시간)
export const setStage = (uuid, id, timestamp) => {
    return stages[uuid].stages.push({ id, timestamp });
}

export const clearStage = (uuid) => {
    stages[uuid] = { score: 0, stages: [] };
}