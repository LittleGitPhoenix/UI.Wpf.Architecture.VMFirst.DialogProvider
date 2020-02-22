using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Models;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Metro.Showcase.ViewModels
{
	class InnerViewModel
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields
		#endregion

		#region Properties

		protected IDefaultDialogManager DefaultDialogManager { get; }

		protected IDialogManager DialogManager { get; }

		#endregion

		#region Enumerations
		#endregion

		#region (De)Constructors

		public InnerViewModel()
		{
			// Save parameters.

			// Initialize fields.
			var viewProvider = new MetroDialogAssemblyViewProvider();
			this.DefaultDialogManager = new DefaultDialogManager(viewProvider);
			this.DialogManager = new DialogManager(viewProvider);

			//// Show a dialog before initialization of the dialog manager.
			//this.ShowConstructorDialog();
		}

		#endregion

		#region Methods

		internal async void Loaded(FrameworkElement view)
		{
			// Initialize the dialog manager.
			this.DialogManager.Initialize(view);

			// Start showing dialogs.
			DialogTask dialogTask = null;

			//dialogTask = this.ShowSimpleDialog();
			//await dialogTask;
			
			//dialogTask = this.ShowMessageInWindow();
			//await dialogTask;
			
			//dialogTask = this.ShowMessageInAdornedView();
			//await dialogTask;

			//dialogTask = this.ShowLongMessageDialog();
			//await dialogTask;

			//dialogTask = this.ShowLongTitleDialog();
			//await dialogTask;

			//dialogTask = this.ShowAutoCancelDialog();
			//await dialogTask;

			//dialogTask = this.ShowCanceledDialog();
			//await dialogTask;

			//var result = await this.ShowMessageWithContentDialog();
			//if (result == DialogResult.Yes)
			//{
			//	dialogTask = await this.ShowComplexDialog();
			//	await dialogTask;
			//}

			//dialogTask = this.ShowDisabledButtonDialog();
			//await dialogTask;

			//dialogTask = this.ShowNestedDialog();
			//await dialogTask;

			//dialogTask = this.ShowDialogThatNeedsClosePermission();
			//await dialogTask;

			//dialogTask = this.ShowWarningDialog();
			//await dialogTask;

			//dialogTask = this.ShowExceptionDialogWithoutException();
			//await dialogTask;

			//dialogTask = this.ShowSimpleExceptionDialog();
			//await dialogTask;

			dialogTask = this.ShowNestedExceptionDialog();
			await dialogTask;

			//dialogTask = this.ShowMultipleExceptionsDialog();
			//await dialogTask;

			//dialogTask = this.ShowContentDialog();
			//await dialogTask;

			//dialogTask = this.ShowContentWithOwnButton();
			//await dialogTask;
		}

		#region Messages

		internal DialogTask ShowConstructorDialog()
		{
			return this.DialogManager.ShowMessage
			(
				title: "Constructor",
				message: "This dialog was created in the constructor and is shown as soon as the dialog handler is fully initialized."
			);
		}

		internal DialogTask ShowSimpleDialog()
		{
			return this.DialogManager.ShowMessage
			(
				message: "This message has no title."
			);
		}

		internal DialogTask ShowMessageInWindow()
		{
			return this.DialogManager.ShowMessage
			(
				title: "Dialog",
				message: "I am displayed within the applications main window.",
				displayLocation: DialogDisplayLocation.Window
			);
		}

		internal DialogTask ShowMessageInAdornedView()
		{
			return this.DialogManager.ShowMessage
			(
				title: "Dialog",
				message: "I am displayed within my adorned view.",
				displayLocation: DialogDisplayLocation.Self
			);
		}
		
		internal DialogTask ShowLongMessageDialog()
		{
			var builder = new StringBuilder();
			builder.AppendLine("断だぽじ確2師テシ市断た量樹むルも月環詐ム傷度ナムヲエ航現ばちうぜ十放ホイヒラ月導ヲ載明ヤソ申本や球京ぐぽな。乗氷がもに京表ホテヱ勢野中ヌ話搭スやレが破初ばぽうめ京7記フシル下分ふへ無責処充クヤ起泊ラヨウノ扱夜クん稿欲3博及司がぴてイ。的リア一弁なげえり陣見で報京ッちなぞ鳴代と都教サモ議紛ルかめ的3以統をめ備質よスわ営悪運ラ世模竜限切ぴなへ。");
			builder.AppendLine("芸前いろらべ判特フツム踊元者権査シノ求作シ常見ナマカソ飛治ぽかけむ教飛なふッス角名レツチ台保レラノホ呉復メ地量へを前聞でぜそ藤態亜才そ。購ラツ趣訃フ門花事ルだ嘉裁負オ震子き察権す約和ね要代んじ古介チリヌ立1女ろ済載日ずゆめレ少同ドこぞ聞鈴ぽ処新とぴさ面僚ろリらく。98合ヒアヨノ事71張スロ件催ルハス度3覧修ワラノ度光異どぐ甘央ぎイッす生読んッどぱ断調様稿愛げぜ。");
			builder.AppendLine("円うよぎ時違15話ヱムサ成典タテスサ数動ワスコ目協機ぎけぱを親65吾4書豆クリス意村よ正法ヱハム外筆廃象護門めク。埼71埼フ的面えこくか毎買ぜレ要写じげラっ吉期でンの江示サ根大ト顔8公カ与次あ電中提クマ近転ヘヤモワ額集ラ味企さぱ相京げくンぞ間仇ゆルし。責ツ弓文び菅製おほ陽会レつぽフ覧惑康フンげべ調古としつ馬極同ヲヨ首話ク幾治ムケハ店員トぽ際技ミ億県しのど。");
			builder.AppendLine("真マハシキ環文ラルつ著口ツ話宏イヘ入分続ーこ新弁けラぎ低書はへ辛連ロ初謝っだ未者ニワオ車供ヘネク選均ト。工心ぼーくル著死者表ぐ県参ユスウラ図3審明ヱシリ首佐メヤワ烈62福ゆッ表結リチヌト分白チケヤネ立販ドち産手軽連英平禁君じげ。違中ノヨムヲ岩者フは文落アヤ使初りもスつ原郎ぐ都覧みちお相心んやみ駆一ゅ髪3球ヱスフ向広ヒスワ罪候をべいな。");
			builder.AppendLine("井ホヲ聞写ぴい般保レつとだ公待通てフぶぱ計書康ナヒヲノ航潤る辞同げーだお込触ろ学方ムル関4提イ掲抽おゆ世効嗅尽措ふンあす。中やめ部使ヤスフキ性査りば短情クずレは応球スコトタ変党イぽ員9貿はも催7刑ロ大前ッよイ学際ゆ意間供ムヲ芸35畑既杉りのや。白ごばのふ賃帰ハネルイ入97味ほ人動薬ネホナ効界ラチ途絡な毎的ヤ悪隠ヌタヤヨ暮花異リー反砂翼羽じとゃび。");
			builder.AppendLine("巡ぴなどが確権ミ象図チ写略告ルれ内23棋党1石ルユアロ改品芸ハコア治勢そ買納移ワツ次区ヌ求彰もい。界あーリべ発国ごなしク者重辺どすリお位聞ヒマ終25界だの応都ヌモ組展豪調レヒナ報際ネキ住高マ人手じきつ裂意近治減ぜゃ。佐よみう残羊起ヘヌナ止落て独供は柔中モエユ並得ンーゆ覚中えうに和校ホウ見白フくー身際経方ん能52要へぴ権表レハエ室侍勃卑みらぱ。");
			builder.AppendLine("合ヲ群現84報どわ札渡エネ同社のきっ身53検記テヘア炉号まへ展演日あ市波冒培すもよ。立応ム豊康せご貿代か解拠ずえ父育前をた亡近ヤヒ軌産ぎ件校ヌテ像局フウタヤ墓区ぎなレラ再腕ラまいが窃氏んちめへ可5仇佗來るえあぜ。安作コシケキ互代ホ妓保し始芸ばのぽ色本脱ロモキ読要らき会6東ぞい米区タ別1検ナユ福氏役ミコカ仕京エマウヒ与所ッさク況応流ニホエ都円くべ聞青針悲毒ねろつぼ。");
			builder.AppendLine("誌し計日のっゆし常益けぶち局最ヤクウ完紀ぎみ際集ルハ療配みさせ提見レテヤ読立責しも遺能条班綸ゃおぐ。惑ヒフレタ米日検リルケサ総善ぞっご歳権メタ各8延キヤヌシ千74賞幌八アスクタ頭慎令締るむげは。較イラ研絡すそたべ演仏テ高直ぜすゆち生形セチ手化キマセ術女源メリケア意検提かやド詳底ま正社ヱイメ義技福レ。");
			builder.AppendLine("宅ゅざ囲磨始こごたぎ福堂1恋テウ旅7心家ソ発1全部マ入犬メ用未ケミヤ前産ヱサ帯贋ぼイっ写報ネイ放者岩破ほさ。服トワホ社38新がへト者地途キヘ要岸カナ亡唄と存市タケヲニ半鉢む地9名葉宿わクほ督犯ッづおん覧投ぱえばー金選オラヘ近柱づのぜ委治ケキオヒ歓所ずべゅふ方異つ話局ぶじらて。");
			builder.AppendLine("順ヒト彼不年わ治38子ミヒエム青1断ず市男ント朝針マニテホ図大けド軽三果ヱネテ襲表覧変エネツ小庁のまけ。取ネツキテ緒留よ西24税点ーらよ豪提ワルセロ取万んろー立小メネ定1開ナミ険独ユ男年オ所種律ンぴこス。数ぶげばか一海う応集ゅち波質をのひ問述をわぶい側秋キセ庁暴ノサニ測角はげどス難夜くで算両くょ館断くを聞資えつり的誌べク備重希示情ル日妻味消戒あもや。");

			return this.DialogManager.ShowMessage
			(
				title: "A long message that automatically uses a vertical scrollbar.",
				message: builder.ToString()
			);
		}

		internal DialogTask ShowLongTitleDialog()
		{
			//return this.DialogManager.ShowMessage
			return this.DefaultDialogManager.ShowMessage
			(
				title: "This title should be long enough so that it may produce a proper line break somewhere within its message. If not, just shrink the window.",
				message: "This message has a title, that is just too long."
			);
		}

		internal DialogTask ShowAutoCancelDialog()
		{
			var cancellationTokenSource = new CancellationTokenSource(delay: TimeSpan.FromSeconds(5));

			return this.DialogManager.ShowMessage
			(
				title: "Auto cancellation",
				message: "This dialog will be canceled after a few seconds.",
				cancellationToken: cancellationTokenSource.Token
			);
		}

		internal DialogTask ShowCanceledDialog()
		{
			var cancellationTokenSource = new CancellationTokenSource();
			cancellationTokenSource.Cancel();

			return this.DialogManager.ShowMessage
			(
				title: "Already canceled",
				message: "This dialog will not show at all, because the external cancellation token is already canceled.",
				cancellationToken: cancellationTokenSource.Token
			);
		}

		internal DialogTask ShowMessageWithContentDialog()
		{
			return this.DialogManager.ShowMessage
			(
				title: "Read carefully",
				message: "Do you believe?",
				contentViewModel: new BelieveViewModel(),
				//buttons: DialogButtons.None
				buttons: DialogButtons.Yes | DialogButtons.No,
				displayLocation: DialogDisplayLocation.Window,
				displayBehavior: DialogDisplayBehavior.Show
			);
		}

		internal Task<DialogTask> ShowComplexDialog()
		{
			return Task.Run(() =>
			{
				var messageDialogModels = new[]
				{
					new MessageDialogModel(identifier: "First", title: "Commandment", message: "I am the Lord your God, who brought you out of the land of Egypt, out of the house of bondage. You shall have no other gods before Me."),
					new MessageDialogModel(identifier: "Second", title: "Commandment", message: "You shall not make for yourself a carved image, or any likeness of anything that is in heaven above, or that is in the earth beneath, or that is in the water under the earth; you shall not bow down to them nor serve them. For I, the Lord your God, am a jealous God, visiting the iniquity of the fathers on the children to the third and fourth generations of those who hate Me, but showing mercy to thousands, to those who love Me and keep My Commandments."),
					new MessageDialogModel(identifier: "Third", title: "Commandment", message: "You shall not take the name of the Lord your God in vain, for the Lord will not hold him guiltless who takes His name in vain"),
					new MessageDialogModel(identifier: "Fourth", title: "Commandment", message: "Remember the Sabbath day, to keep it holy. Six days you shall labor and do all your work, but the seventh day is the Sabbath of the Lord your God. In it you shall do no work: you, nor your son, nor your daughter, nor your male servant, nor your female servant, nor your cattle, nor your stranger who is within your gates. For in six days the Lord made the heavens and the earth, the sea, and all that is in them, and rested the seventh day. Therefore the Lord blessed the Sabbath day and hallowed it."),
					new MessageDialogModel(identifier: "Fifth", title: "Commandment", message: "Honor your father and your mother, that your days may be long upon the land which the Lord your God is giving you."),
					new MessageDialogModel(identifier: "Sixth", title: "Commandment", message: "You shall not murder."),
					new MessageDialogModel(identifier: "Seventh", title: "Commandment", message: "You shall not commit adultery."),
					new MessageDialogModel(identifier: "Eighth", title: "Commandment", message: "You shall not steal."),
					new MessageDialogModel(identifier: "Ninth", title: "Commandment", message: "You shall not bear false witness against your neighbor."),
					new MessageDialogModel(identifier: "Tenth", title: "Commandment", message: "You shall not covet your neighbor's house; you shall not covet your neighbor's wife, nor his male servant, nor his female servant, nor his ox, nor his donkey, nor anything that is your neighbor's.")
				};

				//messageDialogModels = new[]
				//{
				//	new MessageDialogModel(identifier: "Rule #01", title: @"Don't re-invent the wheel", message: @"Too often, our better judgment and ethics play a major role in our job. Is there always a better way to do things? Most often, yes. Should we change it? Most often, no. In larger environments, there are many links to many resources that touch a poorly written piece of code. Without fully understanding the bigger picture, changing 1 variable could result in breaking the entire system. Equally, a stand-alone piece of code that is working in production, most of the time, should never be touched, unless, of course, it is producing undesired results or is performing poorly. Outside of that, let sleeping dogs lie. You'll be glad you did. One small piece of code can quickly turn into a full blown project before you know it. I suppose it's entirely at the mercy or your work load, but consider this before your ambitions take control."),
				//	new MessageDialogModel(identifier: "Rule #02", title: @"Keep things as simple as possible but not simpler", message: @"It's very easy to sit down and start hammering out picturesque OOP, layering the onion with well justified abstraction interfaces, massive exception handling modules, explosive stored procedures, gigantic SSIS/DTS packages, intricate encryption algorithms, and patent worthy designs. Don't do it! Unless you're going to be the code monkey for the company until retirement, you're not only creating a difficult maintenance environment for your limited time there, but you're creating a nightmare environment for the next engineer. If you haven't experienced a mole hill that turns into a mountain, you will, and when that happens, you'll appreciate a simple design that gets the job done. Equally, don't be satisfied with a skeleton design that's vulnerable to security risks or falls victim to end-user misinterpretations. Decide on when enough is enough before you commit to the keyboard."),
				//	new MessageDialogModel(identifier: "Rule #03", title: @"Learn from others mistakes, not your own", message: @"Forums are great place to begin but nothing competes with real world experience. The forums are a good learning environment, even for seasoned engineers, because simply studying the habits of other developers expands your mental dictionaries. Coincidentally, you also learn your own weaknesses while your noticing the mistakes of others. When you can honestly say, ""Hindsight 20/20 is for someone else, "" you've reached programming enlightenment."),
				//	new MessageDialogModel(identifier: "Rule #04", title: @"Performance has no competition.", message: @"Not much explanation required here. There are many current arguments that preach TB hard disks and GB's of RAM as justifications that datatypes and performance-enhanced code are no longer a concern. Wrong. This is completely ignorant. Companies still have budgets and they generally don't include expanding a disk array, adding a processor or a stick of RAM. Quite the contrary, many server settings and applications are being designed to take full advantage of free resources. For example, by default, SQL Server will consume all hardware resources to complete a task. If I had a nickel for every junior DBA that looked at the cpu usage as some form of measurement and scratched their head on the overhead, well, I could afford a retail version of MS Office 2007. :) Code optimization is a specialty on its own. There's a reason there are entire books dedicated to it and it's not entirely due to an author's vanity."),
				//	new MessageDialogModel(identifier: "Rule #05", title: @"Bug free code is more important than clean code.", message: @"This speaks for itself. Many new to the field want to write attractive and easy-to-follow code. There's nothing wrong with that and it is a good practice. However, it is not as important as code without bugs. Before you jump out of your seat, let me comment that both are desired and both can coexist. My point here is that learning to write bug-free  code is a discipline that exalts OOP or n-tier architectures and only time and experience will give you the ability to prevent issues such as implicit conversion errors and overflow exceptions. Now I'm not saying it's not an attainable skill at an early stage of development, I'm simply saying that it is often not the focus of the development life cycle but should be and will be the more you enhance your skills."),
				//	new MessageDialogModel(identifier: "Rule #06", title: @"Begin with an end in mind", message: @"Bet you thought this was a no brainer didn't you? One of the funniest comics I've ever read...it was Dilbert or Far Side, had a manager and 2 programmers. The manager leans over the programmers shoulders and says, ""You 2 start coding and I'll go figure out what they want."" Very Funny. Haha. It happens all the time. Agile/Scrum, Waterfall, and Iterative are overlapping methodologies and are starting to blend colors. Yes projects can be never-ending but give the development stages proper documentation and an identity. This stage identity is a communication medium between you and management, and believe it or not, the ability to communicate and translate programming endeavors to business terms is becoming an increasingly desired skill almost more so than the programming itself. But that's another thread. To close this one, get management or the customer to sign-off on a finite design long before you employ the keyboard. This is your insurance policy and nice folks that don't do this are the ones who work late hours, holidays, and weekends."),
				//	new MessageDialogModel(identifier: "Rule #07", title: @"If it's not broken, don't fix it.", message: @"Just don't do it. Even when you think you can improve upon existing code. Don't. If it works and fulfills the requirement and is in no way offensive (key point here), let it be. Resist the selfish desire to improve the last programmers code. He/she doesn't care and neither should you. You'll have plenty of time and opportunity to write your own code and guess what...the programmer following you will see pinholes in your code that they think need improvement...but the good programmers will let sleeping dogs lie."),
				//	new MessageDialogModel(identifier: "Rule #08", title: @"First seek to understand, then to be understood.", message: @"No Dr. Phil didn't say this but he should. It holds true in all relationships both personally and professionally. Never assume that because you've developed an inventory management system at a previous job that your current employer wants or needs the same design. Although similar in content, the programming can be as different as night and day. Now don't get me wrong, your input is valuable and should be brought forward, but only after your are crystal clear on the requirements. The art of paraphrasing will make you many friends in the programming world because it shows management that you're not only listening intently but you also care about what you do outside of the keyboard. As hinted above, communication skills are slowly superseding programming skills and for good reasons. A brief why...because it's easier to take someone with good communications skills and teach them programming than it is to take a programmer and teach them communication skills. Let's leave it at that for now."),
				//	new MessageDialogModel(identifier: "Rule #09", title: @"There's a place for everything and everything in its place.", message: @"This is a practice that has allowed me to be shortlisted and hired over even the most advanced developers and it's because you must understand and take advantage of the benefits of each environment. For example, SQL Server is designed and optimized to query and reduce records. .Net is not. So if the goal of your code block is to filter a recordset, use the best tool for the job for both performance and optimization reasons. Too many times I've seen SQL developers code procedural-based approaches in a stored procedure because they don't know how to write .net code and .net programmers writing set-based approaches in OOP when it should be in a stored procedure. Read the next sentence carefully. Don't be a victim of your own ignorance but equally, don't bother learning something you'll never use. Simply question and research the best environment for the task at hand and learn something new."),
				//	new MessageDialogModel(identifier: "Rule #10", title: @"Scalability is next to godliness.", message: @"No sooner than you design and deploy an application that you were told and assumed was a throw-away application, it suddenly becomes valuable. It happens all the time. Yes, there are time constraints and other pressures that force us to discard good practices to meet deadlines, but never enough to ignore future developments. It takes only seconds to build a reusable class, compile it to a .dll, and add it to your library or better yet, implement a .dll from an old project because you thought ahead. So instead of copying/pasting or simply rewriting the same function over and over again, start building your own .dll libraries. This habit will allow you to later always ensure that your developments are scalable by giving you a quick and effortless foundation to build from."),
				//};

				var buttonConfigurations = new[]
				{
					new ButtonConfiguration
					(
						caption: "Accept",
						dialogResult: DialogResult.Yes
					),
					new ButtonConfiguration
					(
						caption: "Refuse",
						dialogResult: DialogResult.No,
						callback: () => Trace.WriteLine($"The wrath of god will be directly forwarded to the output window.")
					),
				};

				return this.DialogManager.ShowMessage
				(
					messageDialogModels,
					buttonConfigurations: buttonConfigurations,
					displayLocation: DialogDisplayLocation.Window,
					displayBehavior: DialogDisplayBehavior.Override,
					dialogOptions: DialogOptions.HideTransparencyToggle
				);
			});
		}

		internal DialogTask ShowDisabledButtonDialog()
		{
			var buttonConfigurations = new List<ButtonConfiguration>();
			
			Func<Task<DialogResult>> NoCallback = () =>
			{
				buttonConfigurations[0].IsEnabled = true;
				buttonConfigurations[1].IsEnabled = false;
				return Task.FromResult(DialogResult.None);
			};

			buttonConfigurations.Add
			(
				new ButtonConfiguration
				(
					caption: "Yes",
					dialogResult: DialogResult.Yes
				)
				{
					IsEnabled = false
				}
			);
			buttonConfigurations.Add
			(
				new ButtonConfiguration
				(
					caption: "No",
					callback: NoCallback
				)
				{
					IsEnabled = true
				}
			);

			return this.DialogManager.ShowMessage
			(
				messageModels: new List<MessageDialogModel>()
				{
					new MessageDialogModel(identifier: "First", title: "Not so fast", message: "You must first say 'No' to achieve 'Yes'.")
				},
				buttonConfigurations: buttonConfigurations
			);
		}
		
		internal DialogTask ShowNestedDialog()
		{
			var buttonConfigurations = new[]
			{
				new ButtonConfiguration
				(
					caption: "Yes",
					dialogResult: DialogResult.Yes,
					callback: () =>
					{
						this.DialogManager.ShowWarning("Annoying pop up", "Don't you hate it if this happens.", buttons: DialogButtons.Cancel).Wait();
					}
				)
			};

			return this.DialogManager.ShowMessage
			(
				messageModels: new List<MessageDialogModel>()
				{
					new MessageDialogModel(identifier: String.Empty, title: "Surprise", message: "Do you want to see some adds?")
				},
				buttonConfigurations: buttonConfigurations
			);
		}
		
		internal DialogTask ShowDialogThatNeedsClosePermission()
		{
			//! Both version (synchronous and asynchronous) should work.
			Func<DialogResult> CloseCallback = () =>
			{
				var result = this.DialogManager.ShowWarning("Please confirm", "Do you really want to do this? I really won't ask again.", buttons: DialogButtons.Yes | DialogButtons.No).Result;
				return result == DialogResult.Yes ? DialogResult.Yes : DialogResult.None;
			};
			Func<Task<DialogResult>> CloseCallbackAsync = async () =>
			{
				var result = await this.DialogManager.ShowWarning("Please confirm", "Do you really want to do this? I really won't ask again.", buttons: DialogButtons.Yes | DialogButtons.No);
				return result == DialogResult.Yes ? DialogResult.Yes : DialogResult.None;
			};

			var buttonConfigurations = new[]
			{
				new ButtonConfiguration
				(
					caption: "Close",
					buttonBehavior: DialogButtonBehavior.Cancel,
					//callback: CloseCallback
					callback: CloseCallbackAsync
				)
			};

			return this.DialogManager.ShowMessage
			(
				messageModels: new List<MessageDialogModel>()
				{
					new MessageDialogModel(identifier: "First", title: "Restricted", message: "Acknowledgement is key.")
				},
				buttonConfigurations: buttonConfigurations
			);
		}

		#endregion

		#region Warnings

		internal DialogTask ShowWarningDialog()
		{
			return this.DialogManager.ShowWarning
			(
				title: "Warning",
				message: "Something, some when and somewhere is going to go wrong."
			);
		}

		#endregion

		#region Exceptions

		internal DialogTask ShowExceptionDialogWithoutException()
		{
			return this.DialogManager.ShowException(title: "No Exception", message: "This exception dialog has no exception. It can be used to display known exceptions.");
		}

		internal DialogTask ShowSimpleExceptionDialog()
		{
			try
			{
				this.CreateStackTrace(new AccessViolationException($"Buffer overflow."));
			}
			catch (Exception ex)
			{
				return this.DialogManager.ShowException
				(
					exception: ex,
					title: "51mpl3 3rr0rrrrr...",
					message: "Some thing went wrong along the way."
				);
			}

			return null;
		}

		internal DialogTask ShowNestedExceptionDialog()
		{
			try
			{
				this.CreateStackTrace
				(
					new RootException
					(
						message: $"I am a {nameof(RootException)}.",
						innerException: new MiddleException
						(
							message: "I am not.",
							innerException: new LastException
							(
								message: "I am Groot."
							)
						)
					)
				);
			}
			catch (Exception ex)
			{
				return this.DialogManager.ShowException
				(
					exception: ex,
					title: "N3st3d 3rr0rrrrr...",
					message: "Some thing went wrong along the way.",
					dialogOptions: DialogOptions.AutoExpandStacktrace
				);
			}

			return null;
		}

		internal DialogTask ShowMultipleExceptionsDialog()
		{
			try
			{
				throw new AggregateException
				(
					new ApplicationException($"I am an {nameof(ApplicationException)}."),
					new NotSupportedException("I am not."),
					new BadImageFormatException()
				);
			}
			catch (Exception ex)
			{
				return this.DialogManager.ShowException
				(
					exception: ex,
					title: "Mult1pl3 3rr0rrrrrssss...",
					message: "Some thing went wrong along the way."
				);

				//return this.DialogManager.ShowExceptions
				//(
				//	exceptions: new[] {ex, anotherException},
				//	title: "3rr0rrrrr...",
				//	message: "Some thing went wrong along the way."
				//);
			}
		}

		#region Helper
		
		private DialogTask CreateStackTrace(Exception ex)
			=> this.CreateStackTrace1(ex);

		private DialogTask CreateStackTrace1(Exception ex)
			=> this.CreateStackTrace2(ex);

		private DialogTask CreateStackTrace2(Exception ex)
			=> this.CreateStackTrace3(ex);

		private DialogTask CreateStackTrace3(Exception ex)
		{
			throw ex;
		}

		#endregion
		
		#endregion
		
		#region Content

		internal DialogTask ShowContentDialog()
		{
			var viewModel = new BelieveViewModel();

			var buttonConfigurations = new[]
			{
				new ButtonConfiguration
				(
					caption: "Yep",
					callback: viewModel.OnAccept
				)
				{
					IsEnabled = false
				}
				,
				new ButtonConfiguration
				(
					caption: "Nope",
					callback: viewModel.OnDecline
				)
				{
					IsEnabled = true
				},
			};

			Task.Run(async () =>
			{
				await Task.Delay(3000);
				buttonConfigurations[0].IsEnabled = true;
				buttonConfigurations[1].IsEnabled = false;
			});
			
			return this.DialogManager.ShowContent
			(
				viewModel: viewModel,
				buttonConfigurations: buttonConfigurations
			);
		}

		internal DialogTask ShowContentWithOwnButton()
		{
			var viewModel = new OwnButtonProvidingViewModel();

			var buttonConfigurations = new[]
			{
				new ButtonConfiguration
				(
					caption: "Container Button",
					dialogResult: DialogResult.Yes
				),
			};
			
			return this.DialogManager.ShowContent
			(
				viewModel: viewModel,
				buttonConfigurations: buttonConfigurations
			);
		}

		#endregion

		#endregion
	}
	
	internal class RootException : Exception
	{
		public RootException(string message, Exception innerException) : base(message, innerException) { }
	}

	internal class MiddleException : Exception
	{
		public MiddleException(string message, Exception innerException) : base(message, innerException) { }
	}

	internal class LastException : Exception
	{
		public LastException(string message) : base(message) { }
	}
}