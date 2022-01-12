// PACKAGES
const { beforeAll } = require("@jest/globals");
const axios = require("axios");

// CONF
const appFile = require("./api.json");

// VARS
const apiBaseUrl = appFile.api_base_url.value;
const user = {
  UserName: "user",
};

// TESTS
describe(`GIVEN api is up and running`, () => {
  describe("WHEN calling POST - /sessions", () => {
    let postResponse;
    beforeAll(async () => {
      postResponse = await axios.post(`${apiBaseUrl}/sessions`, user);
    });

    it(`THEN should return CREATED-201`, async () => {
      expect(postResponse.status).toBe(201);
    });
    it(`THEN should contain a token`, async () => {
      expect(postResponse.data.Token).toBeDefined();
    });

    describe("WHEN calling GET - /sessions/{token}", () => {
      let getResponse;
      beforeAll(async () => {
        getResponse = await axios.get(
          `${apiBaseUrl}/sessions/${postResponse.data.Token}`
        );
      });

      it(`THEN should return OK-200`, () => {
        expect(getResponse.status).toBe(200);
      });
      it(`THEN should contain the same userName`, () => {
        expect(getResponse.data.UserName).toBe(user.UserName);
      });
    });
  });
});
