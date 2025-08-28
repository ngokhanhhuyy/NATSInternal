// @ts-check1
/// <reference lib="webworker" />
/**
 * @typedef {object} RequestMessage
 * @property {string} requestId
 * @property {string} pathName
 * @property {string} method
 * @property {{ [key: string]: string }} headers
 * @property {string | null | undefined} body
 * @property {RequestCredentials | undefined} credentials
 */

/**
 * @typedef {object} ResponseMessage
 * @property {string} requestId
 * @property {number} status
 * @property {{ [key: string]: string }} headers
 * @property {string | null | undefined} body
 */

/**
 * Send the message to the websocket server and receive the response.
 * 
 * @callback RequestSender
 * @param {string} pathName 
 * @param {string} method 
 * @param {{ [key: string]: string }} headers 
 * @param {string | null | undefined} body 
 * @param {any | undefined} credentials 
 * @returns {Promise<ResponseMessage>}
 */

/**
 * @typedef {object} WebSocketClient
 * @property {number} readyState
 * @property {RequestSender} sendRequestMessageAsync
 */


/**
 * @typedef {object} Config
 * @property {boolean} interceptApiCalls
 * @property {boolean} interceptAuthenticationCalls
 * @property {boolean} interceptScriptRequests
 */

/** @type {WebSocketClient} */
let wsClient;

/** @type {Config} */
const config = {
    interceptApiCalls: true,
    interceptAuthenticationCalls: true,
    interceptScriptRequests: true,
};

/**
 * @type {Map<string, (response: ResponseMessage) => void>}
 */
const pendingRequests = new Map();

// @ts-ignore
self.addEventListener("activate", (/** @type {ExtendableEvent} */ event) => {
    console.log("Service Worker activated.");
    useWebSocket("ws://localhost:5175").then(client => wsClient = client);
    // @ts-ignore
    event.waitUntil(self.clients.claim());
});

self.addEventListener("message", (event) => {
    if (!event.data) {
        return;
    }

    if (event.data.type === "WS_CONNECTION_RESET_REQUEST") {
        console.log("Websocket re-connection request received");
        if (!wsClient || wsClient.readyState !== WebSocket.OPEN) {
            useWebSocket("ws://localhost:5175").then(client => {
                wsClient = client;
                console.log("Websocket connection re-established");
            });
        }
    } else if (event.data.type === "CONFIG_SUBMIT") {
        console.log("Config change request received");
        const /** @type {Partial<Config>} */ requestedConfig = event.data.config;
        config.interceptApiCalls = !!requestedConfig.interceptApiCalls;
        config.interceptAuthenticationCalls = !!requestedConfig.interceptAuthenticationCalls;
        config.interceptScriptRequests = !!requestedConfig.interceptScriptRequests;
        console.log("Config changed");
    } else if (event.data.type === "CONFIG_REQUEST") {
        console.log("Config get request received");
        console.log(event.source);
        const /** @type {WindowClient} */ eventSource = event.source;
        self.clients.matchAll({ type: eventSource.type, id: eventSource.id }).then()
        event.source?.postMessage({ type: "CONFIG_RESPONSE", config });
    }
})

// @ts-ignore
self.addEventListener("fetch", (/** @type {FetchEvent} */ event) => {
    if (!wsClient?.readyState) {
        return;
    }

    const request = event.request;
    const requestUrl = new URL(event.request.url);
    const requestOrigin = requestUrl.origin;
    const requestPathName = requestUrl.pathname;
    const isTheSameOrigin = requestOrigin === self.location.origin;

    if (!isTheSameOrigin || requestPathName.startsWith("/api/hub")) {
        return;
    }
    
    const isScript = [".js", ".ts", ".vue"]
        .some(extension => requestPathName.endsWith(extension));
    if (isScript && config.interceptScriptRequests) {
        console.log("Intercepting script:", requestPathName);
        event.respondWith((async () => {
            return wsClient.sendRequestMessageAsync(
                requestPathName,
                request.method,
                Object.fromEntries(request.headers.entries()),
                await request.json().catch(() => ""),
                request.credentials)
            .then(responseMessage => new Response(responseMessage.body, {
                    status: responseMessage.status,
                    headers: new Headers(responseMessage.headers),
                }))
            })());
    } else if (requestPathName.startsWith("/api") && config.interceptApiCalls) {
        const isAuthenticationCall = requestPathName.startsWith("/api/authentication/");
        if (isAuthenticationCall && !config.interceptAuthenticationCalls) {
            return;
        }

        console.log("Intercept api call:", requestPathName);
        event.respondWith((async () => {
            return wsClient.sendRequestMessageAsync(
                requestPathName,
                request.method,
                Object.fromEntries(request.headers.entries()),
                await request.json().catch(() => ""),
                request.credentials)
            .then(responseMessage => {
                return new Response(responseMessage.body, {
                    status: responseMessage.status,
                    headers: new Headers(responseMessage.headers),
                });
            })
        })());
    }
});

/**
 * Establish a new websocket connection to the specified server.
 * 
 * @param {string} serverAddress The address of the websocket server.
 * @returns {Promise<WebSocketClient>}
 * An object containing the method the send request as asynchronous operation.
 */
async function useWebSocket(serverAddress) {
    return new Promise(connectionResolve => {
        let socket = new WebSocket(serverAddress);
        socket.onopen = () => {
            console.log("Connected to WebSocket server");
            /**
             * @type {WebSocketClient}
             */
            const client = {
                get readyState() {
                    return socket.readyState;
                },
    
                async sendRequestMessageAsync(pathName, method, headers, body, credentials) {
                    const requestId = crypto.randomUUID();
        
                    /**
                     * @type {Promise<ResponseMessage>}
                     */
                    const responsePromise = new Promise(resolve => {
                        pendingRequests.set(requestId, resolve);
                    });
        
                    /**
                     * @type {RequestMessage}
                     */
                    const requestMessage = {
                        requestId,
                        pathName,
                        method,
                        headers,
                        body,
                        credentials
                    };
                    send(JSON.stringify(requestMessage));
        
                    return responsePromise;
                }
            }
    
            return connectionResolve(client);
        };
    
        socket.onmessage = (event) => {
            // /**
            //  * @type {ResponseMessage}
            //  */
            // const message = JSON.parse(event.data);
            // if (message.requestId && pendingRequests.has(message.requestId)) {
            //     const responseResolve = pendingRequests.get(message.requestId);
            //     responseResolve(message);
            //     pendingRequests.delete(message.requestId);
            // } else {
            //     console.log("Received unexpected message")
            // }
            if (typeof event.data === "string") {
                /**s
                 * @type {ResponseMessage}
                 */
                const responseMessage = JSON.parse(event.data);
                if (responseMessage?.requestId
                        && pendingRequests.has(responseMessage.requestId)) {
                    
                    const responseResolve = pendingRequests.get(responseMessage.requestId);
                    // @ts-ignore
                    responseResolve(responseMessage);
                    pendingRequests.delete(responseMessage.requestId);
                }
            }
            // const data = new Uint8Array(event.data);
            // if (data.length < 4) {
            //     console.error("Received data is too short to contain metadata length");
            //     return;
            // }
    
            // const metadataLength = new DataView(data.buffer).getUint32(0, true);
            // if (data.length < 4 + metadataLength) {
            //     console.error("Received data is too short to contain full metadata and body");
            //     return;
            // }
    
            // const metadataBinary = data.slice(4, 4 + metadataLength);
            // const metadata = JSON.parse(new TextDecoder().decode(metadataBinary));
            // if (!metadata.requestId || !pendingRequests.has(metadata.requestId)) {
            //     console.warn("Received unexpected message");
            //     return;
            // }
    
            // // Extract body
            // const bodyAsBinary = data.slice(4 + metadataLength);
            // const bodyAsBlob = new Blob([bodyAsBinary]);
            // const responseResolve = pendingRequests.get(metadata.requestId);
            // responseResolve({
            //     requestId: metadata.requestId,
            //     status: metadata.status,
            //     headers: metadata.headers,
            //     body: bodyAsBlob,
            // });
    
            // pendingRequests.delete(metadata.requestId);
        };
    
        socket.onerror = (event) => {
            console.error("WebSocket error:", event);
        };
    
        socket.onclose = () => {
            console.warn("WebSocket closed.");
        };
    
        const send = (/** @type {string} */ message) => {
            if (socket.readyState === WebSocket.OPEN) {
                socket.send(message);
            } else {
                console.warn("WebSocket is not open. Message not sent.");
            }
        }
    })
}