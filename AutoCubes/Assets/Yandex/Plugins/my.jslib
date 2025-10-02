mergeInto(LibraryManager.library, {

	Hello: function () {
		window.alert("Hello!");
	},

	GetPlayerData: function () {
		try {
			//window.alert("player = null ? " + player);
		//} else {
			myGameInstance.SendMessage("Yandex", "SetName", player.getName());
			myGameInstance.SendMessage("Yandex", "SetPhoto", player.getPhoto("medium"));
		} catch {
			myGameInstance.SendMessage("Yandex", "SetName", "none");
		}
	},

	RateGame: function () {
		ysdk.feedback.canReview()
			.then(({ value, reason }) => {
				if (value) {
					ysdk.feedback.requestReview()
						.then(({ feedbackSent }) => {
							console.log(feedbackSent);
							});
				} else {
					console.log(reason);
				}
			});
	},

	LoadYandex: function () {
		//window.alert("Load ?!");
		//myGameInstance.SendMessage("MenuControl", "LoadDebug", "Get load info");
		try {
			player.getData().then(_date => {
				const myJSON = JSON.stringify(_date);
				//myGameInstance.SendMessage("MenuControl", "SetPlayerInfo", myJSON);
				myGameInstance.SendMessage("Yandex", "SetPlayerInfo", myJSON);
				//window.alert("GetData ?!" + _date + " myJSON : " + myJSON);
			});
			//window.alert("End Load ?!");
		} catch {
			console.log("GetData error");
		}
	},

	SaveYandex: function (date) {
		//window.alert("Save ?! " + date);
		var dataString = UTF8ToString(date);
		var myObj = JSON.parse(dataString);
		//window.alert("UTF8 : " + dataString + "  myObj : " + myObj);
		try {
		player.setData(myObj);
		//.then(prom => {window.alert("Promise : " + prom);});
		} catch {
			console.log("SetData error");
		}
	},

	SetToLeaderboard: function (score) {
		ysdk.leaderboards.setScore('Score', score);
		//ysdk.getLeaderboards()
		//  .then(lb => {
		    // Без extraData
		//    lb.setLeaderboardScore('Score', score);
		    // С extraData
		    //lb.setLeaderboardScore('leaderboard2021', 120, 'My favourite player!');
		//  });
	},

	GetLeaderboardEntries: function () {
		// Получение 10 топов и по 1 записи вокруг пользователя
    		//lb.getLeaderboardEntries('Score', { quantityTop: 10, includeUser: true, quantityAround: 1 })
		ysdk.leaderboards.getEntries('Score', { quantityTop: 10, includeUser: true, quantityAround: 1 })
      		.then(res => {
			console.log(res);
			var items = res.entries.map((entry) => {
				var item = {
					Rank: entry.rank,
					Score: entry.score,
					Name: entry.player.publicName,
				};
				return item;
			});
			console.log(items);
			var myJsonString = JSON.stringify(items);
			console.log(myJsonString);
			myGameInstance.SendMessage("Yandex", "TranslateLiderboardEntries", myJsonString);
		});
	},

	GetLang: function () {
		var lang = ysdk.environment.i18n.lang;
		var bufferSize = lengthBytesUTF8(lang) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(lang, buffer, bufferSize);
		return buffer;
	},

	ShowAdv : function () {
		ysdk.adv.showFullscreenAdv({
        	callbacks: {
        		onClose: function(wasShown) {
				console.log("-------------- closed -------");
				myGameInstance.SendMessage("Yandex", "CloseAdw");
        		// some action after close
      			},
        		onError: function(error) {
        		// some action on error
      			}
		}
		})
	},

	AddLiveExtern : function (value) {
		ysdk.adv.showRewardedVideo({
        		callbacks: {
        			onOpen: () => {
          				console.log('Video ad open.');
      				},
        			onRewarded: () => {
					console.log('Rewarded!');
					myGameInstance.SendMessage("Yandex", "AddLiveAdw", value);
      				},
        			onClose: () => {
          				console.log('Video ad closed.');
      				}, 
        			onError: (e) => {
          				console.log('Error while open video ad:', e);
      				}
    			}
		})
	},

	AddBonusExtern : function (value) {
		ysdk.adv.showRewardedVideo({
        		callbacks: {
        			onOpen: () => {
          				console.log('Video ad open.');
      				},
        			onRewarded: () => {
					console.log('Rewarded!');
					myGameInstance.SendMessage("Yandex", "AddRewardBonus", value);
					//myGameInstance.SendMessage("Level", "SuccesReward", value);
					//GamePlayStart();
      				},
        			onClose: () => {
          				console.log('Video ad closed.');
					myGameInstance.SendMessage("Yandex", "AdvRewardedClose", value);
					//GamePlayStart();
      				}, 
        			onError: (e) => {
          				console.log('Error while open video ad:', e);
      				}
    			}
		})
	},

	HasFocus : function () {
		if (document.hasFocus()) {
			return true;
		} else {
			return false;
		}
	},


	GamePlayReady : function () {
		ysdk.features.LoadingAPI.ready();
		//ysdk.features.GameplayAPI?.start();
		console.log('GameplayAPI ready.');
	},

	GamePlayStart : function () {
		ysdk.features.GameplayAPI.start();
		//ysdk.features.GameplayAPI?.start();
		console.log('GameplayAPI start.');
	},


	GamePlayStop : function () {
		ysdk.features.GameplayAPI.stop();
		//ysdk.features.GameplayAPI?.stop();
		console.log('GameplayAPI stop.');
	},


});