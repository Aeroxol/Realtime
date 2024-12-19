export const testHandler = (uuid, payload) => {
    console.log(payload);
    return { status: 'success', packetId: 0 };
}
